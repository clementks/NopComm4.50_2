using System;
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

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.NatureOfBusinessName")]
        public string NatureOfBusinessName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }


        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        [NopResourceDisplayName("Admin.Customers.NatureOfBusiness.Fields.SeName")]
        public string SeName { get; set; }

        #endregion
    }
}