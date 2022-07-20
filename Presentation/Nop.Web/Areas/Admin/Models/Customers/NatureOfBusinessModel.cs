using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;


namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a Nature Of Business model
    /// </summary>
    public partial record NatureOfBusinessModel : BaseNopEntityModel, IAclSupportedModel, 
        ILocalizedModel<NatureOfBusinessLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public NatureOfBusinessModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }
            Locales = new List<NatureOfBusinessLocalizedModel>();

            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();

            customerNatureOfBusinessSearchModel = new CustomerNatureOfBusinessSearchModel();
        }

        #endregion

        #region Properties


        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Name")]
        public string NatureOfBusinessName { get; set; }

        public IList<NatureOfBusinessLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Deleted")]
        public bool Deleted { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        //ACL (customer roles)
        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.AclCustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        //store mapping
        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        public CustomerNatureOfBusinessSearchModel customerNatureOfBusinessSearchModel { get; set; }


        #endregion
    }

    public partial record NatureOfBusinessLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Name")]
        public string NatureOfBusinessName { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.SeName")]
        public string SeName { get; set; }

    }
}