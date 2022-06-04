using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents nature of business model to add to the customer
    /// </summary>
    public partial record AddNatureOfBusinessToCustomerModel : BaseNopModel
    {
        #region Ctor

        public AddNatureOfBusinessToCustomerModel()
        {
            SelectedNatureOfBusinessIds = new List<int>();
        }
        #endregion

        #region Properties

        public IList<int> SelectedNatureOfBusinessIds { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        #endregion
    }
}