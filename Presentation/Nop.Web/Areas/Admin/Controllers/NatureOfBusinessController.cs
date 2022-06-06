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
            await _customerService.UpdateCustomerNatureOfBusinessAsync(natureOfBusiness);

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

        [HttpPost]
        public virtual async Task<IActionResult> NatureOfBusinessList(NatureOfBusinessCustomerSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
                return await AccessDeniedDataTablesJson();

            //try to get a customer with the specified user name
            var natureOfBusiness = await _customerService.GetNatureOfBusinessByIdsAsync(searchModel.NatureOfBusinessId)
                ?? throw new ArgumentException("No nature of business found with the specified id");

            //prepare model
            var model = await _customerModelFactory.PrepareCustomerNatureOfBusinessModelListAsync(searchModel, natureOfBusiness);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> NatureOfBusinessUpdate(NatureOfBusinessCustomerModel model)
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
            var model = await _customerModelFactory.PrepareAddProductToManufacturerListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual async Task<IActionResult> NatureOfBusinessAddPopup(AddNatureOfBusinessToCustomerModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNatureOfBusiness))
                return AccessDeniedView();

            //get selected products
            var selectedProducts = await _customerService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            if (selectedProducts.Any())
            {
                var existingProductmanufacturers = await _customerService
                    .GetProductManufacturersByManufacturerIdAsync(model.ManufacturerId, showHidden: true);
                foreach (var product in selectedProducts)
                {
                    //whether product manufacturer with such parameters already exists
                    if (_customerService.FindProductManufacturer(existingProductmanufacturers, product.Id, model.ManufacturerId) != null)
                        continue;

                    //insert the new product manufacturer mapping
                    await _customerService.InsertProductManufacturerAsync(new CustomerNatureOfBusiness
                    {
                        ManufacturerId = model.ManufacturerId,
                        ProductId = product.Id,
                        IsFeaturedProduct = false,
                        DisplayOrder = 1
                    });
                }
            }

            ViewBag.RefreshPage = true;

            return View(new AddProductToManufacturerSearchModel());
        }


        #endregion



    }
}