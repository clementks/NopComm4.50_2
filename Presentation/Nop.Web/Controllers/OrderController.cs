using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Services.Logging;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Services.Messages;
using Nop.Services.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Media;
using Nop.Web.Models.Order;


namespace Nop.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public partial class OrderController : BasePublicController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPdfService _pdfService;
        private readonly IShipmentService _shipmentService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly INopFileProvider _fileProvider;
        private readonly IPictureService _pictureService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly OrderSettings _orderSettings;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Ctor

        public OrderController(ICustomerService customerService,
            IOrderModelFactory orderModelFactory,
            IOrderProcessingService orderProcessingService, 
            IOrderService orderService, 
            IPaymentService paymentService, 
            IPdfService pdfService,
            IShipmentService shipmentService, 
            IWebHelper webHelper,
            IWorkContext workContext,
            RewardPointsSettings rewardPointsSettings,
            INotificationService notificationService,
            ILocalizationService localizationService,
            INopFileProvider fileProvider,
            IPictureService pictureService,
            IWorkflowMessageService workflowMessageService,
            OrderSettings orderSettings,
            ICustomerActivityService customerActivityService)
        {
            _customerService = customerService;
            _orderModelFactory = orderModelFactory;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _paymentService = paymentService;
            _pdfService = pdfService;
            _shipmentService = shipmentService;
            _webHelper = webHelper;
            _workContext = workContext;
            _rewardPointsSettings = rewardPointsSettings;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _fileProvider = fileProvider;
            _pictureService = pictureService;
            _workflowMessageService = workflowMessageService;
            _orderSettings = orderSettings;
            _customerActivityService = customerActivityService;
        }

        #endregion

        #region Methods

        //My account / Orders
        public virtual async Task<IActionResult> CustomerOrders()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var model = await _orderModelFactory.PrepareCustomerOrderListModelAsync();
            return View(model);
        }

        [HttpPost, ActionName("CustomerCancelOrder")]
        // Cancellation of order in the payment summary page
        public virtual async Task<IActionResult> CustomerCancelOrder(int orderId)
        {

            try
            {

                //try to get an order with the specified id
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                    return RedirectToAction("List");

                //a vendor does not have access to this functionality
                if (await _workContext.GetCurrentVendorAsync() != null)
                    return RedirectToAction("Edit", "Order", new { orderId });

                var customer = await _workContext.GetCurrentCustomerAsync();

                if (order == null || order.Deleted || customer.Id != order.CustomerId)
                    return Challenge();
                // order cancellation processing & notification service set=true
                if (_orderProcessingService.CanCancelOrder(order))
                {

                    await _orderProcessingService.CancelOrderAsync(order, true);
                    await LogEditOrderAsync(order.Id);

                    //if (orderCompletedCustomerNotificationQueuedEmailIds.Any())
                    //    await AddOrderNoteAsync(order, $"\"Order completed\" email (to customer) has been queued. Queued email identifiers: {string.Join(", ", orderCompletedCustomerNotificationQueuedEmailIds)}.");

                    return Json(new
                    {
                        cancelOk = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        cancelOk = false
                    });
                }

                //prepare model
                //var model = await _orderModelFactory.PaymentSummaryModelAsync();
                //return View(model);


            }
            catch (Exception exc)
            {
                //prepare model
                //var model = await _orderModelFactory.PaymentSummaryModelAsync();
                return Json(new
                {
                    cancelOk = false
                });

                await _notificationService.ErrorNotificationAsync(exc);
                //return View(model);

            }
        }

        // add by clement on 01-Apr-2022
        public virtual async Task<IActionResult> paymentSummary()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var model = await _orderModelFactory.PaymentSummaryModelAsync();
            return View(model);
        }

        protected virtual async Task LogEditOrderAsync(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            await _customerActivityService.InsertActivityAsync("EditOrder",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditOrder"), order.CustomOrderNumber), order);
        }

        /// <summary>
        /// Add order note
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="note">Note text</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task AddOrderNoteAsync(Order order, string note)
        {
            await _orderService.InsertOrderNoteAsync(new OrderNote
            {
                OrderId = order.Id,
                Note = note,
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
        }

        [HttpPost, ActionName("paymentSummary")]
        public virtual async Task<IActionResult> paymentSummary(CustomerOrderListModel orderList)
        {
            try
            {

                string storePaymentImagePath = _fileProvider.GetAbsolutePath(string.Format(NopCommonDefaults.ManualPaymentProofImagePath));
                var model = await _orderModelFactory.PaymentSummaryModelAsync();
                if (ModelState.IsValid)
                {
                    
                    foreach (var orderItems in orderList.Orders)
                    {
                        if (orderItems.PaymentImagePhoto != null)
                        {

                            string picturefileName = orderItems.PaymentImagePhoto.FileName;
                            var uploadPaymentImagePath = _fileProvider.Combine(storePaymentImagePath, picturefileName);
                            using (var fileStream = new FileStream(uploadPaymentImagePath, FileMode.Create))
                            {
                                await orderItems.PaymentImagePhoto.CopyToAsync(fileStream);
                            }
              
                            var order = await _orderService.GetOrderByCustomOrderNumberAsync(orderItems.CustomOrderNumber);
                            order.PaymentImageUrl = picturefileName;
                            order.PaymentImageSubmitOnUtc = DateTime.UtcNow;
                            await _orderService.UpdateOrderAsync(order);
                            await _workflowMessageService.SendOrderPaidImageUploadNotificationAsync(order, order.CustomerLanguageId, uploadPaymentImagePath, picturefileName);

                            //if (SendOrderPaidImageUploadNotification.Any())
                            //    await AddOrderNoteAsync(order, $"\"Order Paid Image Upload\" email (to Store Owner) has been queued. Queued email identifiers: {string.Join(", ", SendOrderPaidImageUploadNotification)}.");

                        }

                    }

                    _notificationService.SuccessNotification("Upload Success");
                    return View(model);

                }

                var message = "No valid Photo image to Upload!";
                _notificationService.UploadErrorNotification(message);
                return View(model);
            }
            catch (Exception exc)
            {
               
                var model = await _orderModelFactory.PaymentSummaryModelAsync();
                await _notificationService.ErrorNotificationAsync(exc);
                return View(model);


            }

        }

       



        //My account / Orders / Cancel recurring order
        [HttpPost, ActionName("CustomerOrders")]
        [FormValueRequired(FormValueRequirement.StartsWith, "cancelRecurringPayment")]
        public virtual async Task<IActionResult> CancelRecurringPayment(IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            foreach (var formValue in form.Keys)
                if (formValue.StartsWith("cancelRecurringPayment", StringComparison.InvariantCultureIgnoreCase))
                    recurringPaymentId = Convert.ToInt32(formValue["cancelRecurringPayment".Length..]);

            var recurringPayment = await _orderService.GetRecurringPaymentByIdAsync(recurringPaymentId);
            if (recurringPayment == null)
            {
                return RedirectToRoute("CustomerOrders");
            }

            if (await _orderProcessingService.CanCancelRecurringPaymentAsync(customer, recurringPayment))
            {
                var errors = await _orderProcessingService.CancelRecurringPaymentAsync(recurringPayment);

                var model = await _orderModelFactory.PrepareCustomerOrderListModelAsync();
                model.RecurringPaymentErrors = errors;

                return View(model);
            }

            return RedirectToRoute("CustomerOrders");
        }

        //My account / Orders / Retry last recurring order
        [HttpPost, ActionName("CustomerOrders")]
        [FormValueRequired(FormValueRequirement.StartsWith, "retryLastPayment")]
        public virtual async Task<IActionResult> RetryLastRecurringPayment(IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            if (!form.Keys.Any(formValue => formValue.StartsWith("retryLastPayment", StringComparison.InvariantCultureIgnoreCase) &&
                int.TryParse(formValue[(formValue.IndexOf('_') + 1)..], out recurringPaymentId)))
            {
                return RedirectToRoute("CustomerOrders");
            }

            var recurringPayment = await _orderService.GetRecurringPaymentByIdAsync(recurringPaymentId);
            if (recurringPayment == null)
                return RedirectToRoute("CustomerOrders");

            if (!await _orderProcessingService.CanRetryLastRecurringPaymentAsync(customer, recurringPayment))
                return RedirectToRoute("CustomerOrders");

            var errors = await _orderProcessingService.ProcessNextRecurringPaymentAsync(recurringPayment);
            var model = await _orderModelFactory.PrepareCustomerOrderListModelAsync();
            model.RecurringPaymentErrors = errors.ToList();

            return View(model);
        }

        //My account / Reward points
        public virtual async Task<IActionResult> CustomerRewardPoints(int? pageNumber)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            if (!_rewardPointsSettings.Enabled)
                return RedirectToRoute("CustomerInfo");

            var model = await _orderModelFactory.PrepareCustomerRewardPointsAsync(pageNumber);
            return View(model);
        }

        //My account / Order details page
        public virtual async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            var model = await _orderModelFactory.PrepareOrderDetailsModelAsync(order);
            return View(model);
        }

        //My account / Order details page / Print
        public virtual async Task<IActionResult> PrintOrderDetails(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            var model = await _orderModelFactory.PrepareOrderDetailsModelAsync(order);
            model.PrintMode = true;

            return View("Details", model);
        }

        //My account / Order details page / PDF invoice
        [CheckLanguageSeoCode(true)]
        public virtual async Task<IActionResult> GetPdfInvoice(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            var orders = new List<Order>
            {
                order
            };
            byte[] bytes;
            await using (var stream = new MemoryStream())
            {
                await _pdfService.PrintOrdersToPdfAsync(stream, orders, (await _workContext.GetWorkingLanguageAsync()).Id);
                bytes = stream.ToArray();
            }
            return File(bytes, MimeTypes.ApplicationPdf, $"order_{order.CustomOrderNumber}.pdf");
        }

        //My account / Order details page / re-order
        public virtual async Task<IActionResult> ReOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            await _orderProcessingService.ReOrderAsync(order);
            return RedirectToRoute("ShoppingCart");
        }

        //My account / Order details page / Complete payment
        [HttpPost, ActionName("Details")]
        
        [FormValueRequired("repost-payment")]
        public virtual async Task<IActionResult> RePostPayment(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            if (!await _paymentService.CanRePostProcessPaymentAsync(order))
                return RedirectToRoute("OrderDetails", new { orderId = orderId });

            var postProcessPaymentRequest = new PostProcessPaymentRequest
            {
                Order = order
            };
            await _paymentService.PostProcessPaymentAsync(postProcessPaymentRequest);

            if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
            {
                //redirection or POST has been done in PostProcessPayment
                return Content("Redirected");
            }

            //if no redirection has been done (to a third-party payment page)
            //theoretically it's not possible
            return RedirectToRoute("OrderDetails", new { orderId = orderId });
        }

        //My account / Order details page / Shipment details page
        public virtual async Task<IActionResult> ShipmentDetails(int shipmentId)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId);
            if (shipment == null)
                return Challenge();

            var order = await _orderService.GetOrderByIdAsync(shipment.OrderId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            var model = await _orderModelFactory.PrepareShipmentDetailsModelAsync(shipment);
            return View(model);
        }
        
        #endregion
    }
}