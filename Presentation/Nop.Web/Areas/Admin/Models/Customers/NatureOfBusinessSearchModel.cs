using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a nature of business search model
    /// </summary>
    public partial record NatureOfBusinessSearchModel : BaseSearchModel
    {
        #region Ctor

        public NatureOfBusinessSearchModel()
        {
            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Customers.Customers.List.CustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }

        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        [NopResourceDisplayName("Admin.Customer.NatureOfBusiness.List.SearchNatureOfBusinessName")]
        public string SearchNatureOfBusinessName { get; set; }

        [NopResourceDisplayName("Admin.Customer.NatureOfBusiness.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }
        #endregion
    }
}