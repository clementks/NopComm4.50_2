using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a nature of business search model
    /// </summary>
    public partial record NatureOfBusinessCustomerSearchModel : BaseSearchModel
    {
        #region Properties

        public int NatureOfBusinessId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        #endregion
    }
}