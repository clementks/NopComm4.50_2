using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;



namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents customer to add to the nature of business model 
    /// </summary>
    public partial record AddCustomerToNatureOfBusinessModel : BaseNopModel
    {
        #region Ctor

        public AddCustomerToNatureOfBusinessModel()
        {
            SelectedUserNames = new List<string>();

            SelectedCustomerIds = new List<int>();

        }
        #endregion

        #region Properties

        public IList<string> SelectedUserNames { get; set; }

        public string SelectedUserName { get; set; }

        public string NatureOfBusinessName { get; set; }

        public IList<int> SelectedCustomerIds { get; set; }

        #endregion
    }
}