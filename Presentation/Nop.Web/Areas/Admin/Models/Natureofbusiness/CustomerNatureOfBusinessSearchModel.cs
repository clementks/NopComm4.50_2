using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Natureofbusiness
{ 
    /// <summary>
    /// Represents a nature of business search model
    /// </summary>
    public partial record CustomerNatureOfBusinessSearchModel : BaseSearchModel
    {
        #region Properties

        public int NatureOfBusinessId { get; set; }

        public string SearchNatureOfBusinessName { get; set; }

        public string SearchUsername { get; set; }

        public string SearchEmail { get; set; }


        #endregion
    }
}