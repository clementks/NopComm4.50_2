using System;
using Nop.Core.Domain.Common;
//using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;


namespace Nop.Core.Domain.Natureofbusinesses
{
    /// <summary>
    /// Represents Nature Of Business
    /// </summary>
    public partial class Natureofbusiness : BaseEntity, ISlugSupported, ISoftDeletedEntity
    { 
        /// <summary>
        /// Gets or sets the names on nature of business
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the nature of Business identifier
        /// </summary>
        public int NatureOfBusinessId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
        //public bool SubjectToAcl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        //public bool LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }



    }
}