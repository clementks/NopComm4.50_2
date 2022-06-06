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
            AvailableNatureOfBusiness = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.customers.List.SearchUserName")]
        public string SearchUserName { get; set; }

        public IList<SelectListItem> AvailableNatureOfBusiness { get; set; }

        #endregion
    }
}