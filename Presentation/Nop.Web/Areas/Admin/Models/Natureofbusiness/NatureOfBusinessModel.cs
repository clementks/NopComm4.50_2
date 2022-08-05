using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;


namespace Nop.Web.Areas.Admin.Models.Natureofbusiness
{
    /// <summary>
    /// Represents a Nature Of Business model
    /// </summary>
    public partial record NatureOfBusinessModel : BaseNopEntityModel
    {
        #region Ctor

        public NatureOfBusinessModel()
        {
            //if (PageSize < 1)
            //{
            //    PageSize = 5;
            //}
            //Locales = new List<NatureOfBusinessLocalizedModel>();

            //SelectedCustomerRoleIds = new List<int>();
            //AvailableCustomerRoles = new List<SelectListItem>();

            //SelectedStoreIds = new List<int>();
            //AvailableStores = new List<SelectListItem>();

            //CustomerNatureOfBusinessSearchModel = new CustomerNatureOfBusinessSearchModel();
        }

        #endregion

        #region Properties


        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Name")]
        public string Name { get; set; }


        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Id")]
        public int NatureOfBusinessId { get; set; }

        //public IList<NatureOfBusinessLocalizedModel> Locales { get; set; }

        //[NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.PageSize")]
        //public int PageSize { get; set; }

        //[NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Deleted")]
        //public bool Deleted { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.CreateOn")]
        [Column(TypeName = "Date")]
        public DateTime CreatedOnUtc { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.UpdateOn")]
        [Column(TypeName = "Date")]
        public DateTime UpdatedOnUtc { get; set; }

        //public NatureOfBusinessSearchModel NatureOfBusinessSearchModel { get; set; }

        //ACL (customer roles)
        //[NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.AclCustomerRoles")]
        //public IList<int> SelectedCustomerRoleIds { get; set; }
        //public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        ////store mapping
        //[NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.LimitedToStores")]
        //public IList<int> SelectedStoreIds { get; set; }
        //public IList<SelectListItem> AvailableStores { get; set; }



        #endregion
    }

    //public partial record NatureOfBusinessLocalizedModel : ILocalizedLocaleModel
    //{
    //    public int LanguageId { get; set; }

    //    [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Name")]
    //    public string Name { get; set; }

    //    public int NatureOfBusinessId { get; set; }

    //    [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Description")]
    //    public string Description { get; set; }

    //    [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.MetaKeywords")]
    //    public string MetaKeywords { get; set; }

    //    [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.MetaDescription")]
    //    public string MetaDescription { get; set; }

    //    [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.SeName")]
    //    public string SeName { get; set; }

    //}
 }