using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;



namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents customer to add to the nature of business model 
    /// </summary>
    public partial record AddCustomerToNatureOfBusinessSearchModel : BaseSearchModel
    {
        #region Ctor

        public AddCustomerToNatureOfBusinessSearchModel()
        {
            AvailableNatureOfBusinessName = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.customers.List.SearchUserName")]

        public IList<SelectListItem> AvailableNatureOfBusinessName { get; set; }

        public int SearchCustomerId { get; set; }

        [NopResourceDisplayName("Admin.Customer.NatureOfBusiness.List.SearchNatureOfBusinessName")]
        public string SearchNatureOfBusinessName { get; set; }

        public string SearchUserName { get; set; }

        public string SearchEmail { get; set; }



        #endregion
    }
}