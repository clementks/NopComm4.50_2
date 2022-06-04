using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents Nature Of Business associated with Customer model
    /// </summary>
    public partial record NatureOfBusinessCustomerModel : BaseNopEntityModel
    {
        #region Properties

        public int NatureOfBusinessId { get; set; }

        public int customerId { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.Name")]
        public string NatureOfBusiness { get; set; }


        #endregion
    }
}