using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Natureofbusiness
{
    /// <summary>
    /// Represents a nature of business search model
    /// </summary>
    public partial record NatureOfBusinessSearchModel : BaseSearchModel
    {
        #region Ctor

        public NatureOfBusinessSearchModel()
        {

            NatureOfBusinessIds = new List<int>();
            //AvailableUserNames = new List<SelectListItem>();
            //AvailableEmails = new List<SelectListItem>();
            //AvailableNatureOfBusiness = new List<SelectListItem>();

        }

        #endregion

        #region Properties

        //[NopResourceDisplayName("Admin.Customers.Customers.List.CustomerRoles")]
        public IList<int> NatureOfBusinessIds { get; set; }

        //public IList<SelectListItem> AvailableUserNames { get; set; }

        //public IList<SelectListItem> AvailableEmails { get; set; }

        //public IList<SelectListItem> AvailableNatureOfBusiness { get; set; }



        [NopResourceDisplayName("Admin.Customer.NatureOfBusiness.List.SearchNatureOfBusinessName")]
        public string SearchNatureOfBusinessName { get; set; }


        //public string SearchUserName { get; set; }

        //public string SearchEmail { get; set; }

        //public bool NatureOfBusinessEnabled { get; set; }

        //public IList<string> NatureOfBusinessNames { get; set; }

        //public IList<bool> Published { get; set; }

        //public IList<DateTime> CreatedOn { get; set; }

        #endregion
    }
}