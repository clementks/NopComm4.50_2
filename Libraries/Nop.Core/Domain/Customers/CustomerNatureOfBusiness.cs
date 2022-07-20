using System;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.Customers
{
    /// <summary>
    /// Represents Customer & Nature Of Business mapping
    /// </summary>
    public partial class CustomerNatureOfBusiness : BaseEntity
    {


        /// <summary>
        /// Gets or sets the nature of Business identifier
        /// </summary>
        public int NatureOfBusinessId { get; set; }

        /// <summary>
        /// Gets or sets the nature of Business name
        /// </summary>
        public string NatureOfBusinessName { get; set; }

        /// <summary>
        /// Gets or sets the customer id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }


        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }


    }
}