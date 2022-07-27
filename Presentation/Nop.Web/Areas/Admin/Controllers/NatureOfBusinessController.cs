using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Helpers;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class NatureOfBusinessController : BaseAdminController
    {
        #region Fields


        private readonly CustomerSettings _customerSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IAclService _aclService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerService _customerService;
        private readonly IDiscountService _discountService;
        private readonly IExportManager _exportManager;
        private readonly IImportManager _importManager;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IWorkContext _workContext;


        #endregion

        #region Ctor

        public NatureOfBusinessController(CustomerSettings customerSettings,
            DateTimeSettings dateTimeSettings,
            EmailAccountSettings emailAccountSettings,
            IAclService aclService,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IDiscountService discountService,
            IExportManager exportManager,
            IImportManager importManager,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ICustomerModelFactory customerModelFactory,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IProductService productService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            IWorkflowMessageService workflowMessageService,
            IWorkContext workContext)
        {
            _customerSettings = customerSettings;
            _dateTimeSettings = dateTimeSettings;
            _emailAccountSettings = emailAccountSettings;
            _aclService = aclService;
            _customerActivityService = customerActivityService;
            _customerService = customerService;
            _discountService = discountService;
            _exportManager = exportManager;
            _importManager = importManager;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _customerModelFactory = customerModelFactory;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _productService = productService;
            _storeMappingService = storeMappingService;
            _storeService = storeService;
            _urlRecordService = urlRecordService;
            _workflowMessageService = workflowMessageService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        protected virtual async Task UpdateLocalesAsync(NatureOfBusiness natureOfBusiness, NatureOfBusinessModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(natureOfBusiness,
                    x => x.NatureOfBusinessName,
                    localized.NatureOfBusinessName,
                    localized.LanguageId);

                //search engine name
                //var seName = await _urlRecordService.ValidateSeNameAsync(natureOfBusiness, localized.SeName, localized.NatureOfBusinessName, false);
                //await _urlRecordService.SaveSlugAsync(natureOfBusiness, seName, localized.LanguageId);
            }
        }

        protected virtual async Task SaveNatureOfBusinessAclAsync(NatureOfBusiness natureOfBusiness, NatureOfBusinessModel model)
        {
            natureOfBusiness.SubjectToAcl = model.SelectedCustomerRoleIds.Any();
            await _customerService.UpdateNatureOfBusinessAsync(natureOfBusiness);

            var existingAclRecords = await _aclService.GetAclRecordsAsync(natureOfBusiness);
            var allCustomerRoles = await _customerService.GetAllCustomerRolesAsync(true);
            foreach (var customerRole in allCustomerRoles)
            {
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                {
                    //new role
                    if (!existingAclRecords.Any(acl => acl.CustomerRoleId == customerRole.Id))
                        await _aclService.InsertAclRecordAsync(natureOfBusiness, customerRole.Id);
                }
                else
                {
                    //remove role
                    var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.CustomerRoleId == customerRole.Id);
                    if (aclRecordToDelete != null)
                        await _aclService.DeleteAclRecordAsync(aclRecordToDelete);
                }
            }
        }


        #endregion


        #region nature of business


        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = await _customerModelFactory.PrepareNatureOfBusinessSearchModelAsync(new NatureOfBusinessSearchModel());

            return View(model);
        }

        //[HttpPost]
        //public virtual async Task<IActionResult> CustomerList(CustomerSearchModel searchModel)
        //{
        //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
        //        return await AccessDeniedDataTablesJson();

        //    //prepare model
        //    var model = await _customerModelFactory.PrepareCustomerListModelAsync(searchModel);

        //    return Json(model);
        //}


        [HttpPost]
        public virtual async Task<IActionResult> List(NatureOfBusinessSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return await AccessDeniedDataTablesJson();

            //try to get nature of business with the specified nature of business name
            //var natureOfBusiness = await _customerService.GetNatureOfBusinessByNameAsync(searchModel.SearchNatureOfBusinessName)
            //    ?? throw new ArgumentException("No nature of business found with the specified name");

            //prepare model
            var model = await _customerModelFactory.PrepareNatureOfBusinessListModelAsync(searchModel);
            

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> NatureOfBusinessUpdate(CustomerNatureOfBusinessModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
                return AccessDeniedView();

            //try to get a customer & nature of business with the specified id
            var customerNatureOfBusiness = await _customerService.GetCustomerNatureOfBusinessByIdAsync(model.Id)
                ?? throw new ArgumentException("No customer & nature of business mapping found with the specified id");

            //fill entity from model
            customerNatureOfBusiness = model.ToEntity(customerNatureOfBusiness);
            await _customerService.UpdateCustomerNatureOfBusinessAsync(customerNatureOfBusiness);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual async Task<IActionResult> NatureOfBusinessDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
                return AccessDeniedView();

            //try to get a product manufacturer with the specified id
            var customerNatureOfBusiness = await _customerService.GetCustomerNatureOfBusinessByIdAsync(id)
                ?? throw new ArgumentException("No customer & nature of business mapping found with the specified id");

            await _customerService.DeleteCustomerNatureOfBusinessAsync(customerNatureOfBusiness);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> NatureOfBusinessAddPopup(int natureOfBusinessId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
                return AccessDeniedView();

            //prepare model
            var model = await _customerModelFactory.PrepareAddCustomerToNatureOfBusinessSearchModelAsync(new AddCustomerToNatureOfBusinessSearchModel());

            return View(model);
        }


        [HttpPost]
        public virtual async Task<IActionResult> NatureOfBusinessAddPopupList(AddCustomerToNatureOfBusinessSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _customerModelFactory.PrepareAddCustomerToNatureOfBusinessListModelAsync(searchModel);

            return Json(model);
        }

        //[HttpPost]
        //[FormValueRequired("save")]
        //public virtual async Task<IActionResult> NatureOfBusinessAddPopup(AddCustomerToNatureOfBusinessModel model)
        //{
        //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
        //        return AccessDeniedView();

        //    //get selected nature of business
        //    var selectedCustomers = await _customerService.GetCustomersByIdsAsync(model.SelectedCustomerIds.ToArray());
        //    if (selectedCustomers.Any())
        //    {
        //        var existingCustomerNatureOfBusiness = await _customerService
        //            .GetCustomerNatureOfBusinessByUserNameAsync(model.SelectedUserName, showHidden: false);
        //        foreach (var selCustomer in selectedCustomers)
        //        {
        //            //whether Customer & Nature Of Business with such parameters already exists
        //            if (_customerService.FindCustomerNatureOfBusiness(existingCustomerNatureOfBusiness, selCustomer.Username) != null)
        //                continue;

        //            //insert Customer Nature Of Business mapping
        //            await _customerService.InsertCustomerNatureOfBusinessAsync(new CustomerNatureOfBusiness
        //            {
        //                NatureOfBusinessName = model.NatureOfBusinessName,
        //                Username = model.SelectedUserName,
        //                DisplayOrder = 1,
        //                Published = true
        //            });
        //        }
        //    }

        //    ViewBag.RefreshPage = true;

        //    return View(new AddCustomerToNatureOfBusinessModel());
        //}

        [HttpPost]
        public virtual async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            if (selectedIds == null || selectedIds.Count == 0)
                return NoContent();

            var natureOfBusinesses = await _customerService.GetNatureOfBusinessByIdsAsync(selectedIds.ToArray());
            await _customerService.DeleteNatureOfBusinessesAsync(natureOfBusinesses);

            var locale = await _localizationService.GetResourceAsync("ActivityLog.DeleteNatureOfBusiness");
            foreach (var natureOfBusiness in natureOfBusinesses)
            {
                //activity log
                await _customerActivityService.InsertActivityAsync("DeleteNatureOfBusiness", string.Format(locale, natureOfBusiness.NatureOfBusinessName), natureOfBusiness);
            }

            return Json(new { Result = true });
        }



        #endregion

        #region Export / Import

        public virtual async Task<IActionResult> ExportXml()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageManufacturers))
                return AccessDeniedView();

            try
            {
                var natureOfBusiness = await _customerService.GetAllNatureOfBusinessAsync(showHidden: true);
                var xml = await _exportManager.ExportNatureOfBusinessToXmlAsync(natureOfBusiness);
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", "natureOfBusiness.xml");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToAction("List");
            }
        }

        public virtual async Task<IActionResult> ExportXlsx()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageManufacturers))
                return AccessDeniedView();

            try
            {
                var bytes = await _exportManager.ExportNatureOfBusinessesToXlsxAsync((await _customerService.GetAllNatureOfBusinessAsync(showHidden: true)).Where(p => !p.Deleted));

                return File(bytes, MimeTypes.TextXlsx, "natureOfBusiness.xlsx");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> ImportFromXlsx(IFormFile importexcelfile)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageManufacturers))
                return AccessDeniedView();

            //a vendor cannot import manufacturers
            if (await _workContext.GetCurrentVendorAsync() != null)
                return AccessDeniedView();

            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    await _importManager.ImportManufacturersFromXlsxAsync(importexcelfile.OpenReadStream());
                }
                else
                {
                    _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Common.UploadFile"));
                    return RedirectToAction("List");
                }

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.NatureOfBusiness.Imported"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToAction("List");
            }
        }

        #endregion



    }
}