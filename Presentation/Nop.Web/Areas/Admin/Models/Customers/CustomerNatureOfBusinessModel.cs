using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents Nature Of Business associated with Customer model
    /// </summary>
    public partial record CustomerNatureOfBusinessModel : BaseNopEntityModel
    {
        #region Properties

        public int NatureOfBusinessId { get; set; }

        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.NatureOfBusinessName")]
        public string NatureOfBusiness { get; set; }


        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}