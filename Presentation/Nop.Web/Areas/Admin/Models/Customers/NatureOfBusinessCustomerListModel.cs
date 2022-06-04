using Nop.Web.Framework.Models;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents Nature Of Business associated with Customer list model
    /// </summary>NatureOfBusinessCustomer 
    public partial record NatureOfBusinessCustomerListModel : BasePagedListModel<NatureOfBusinessCustomerModel>
    {
    }
}