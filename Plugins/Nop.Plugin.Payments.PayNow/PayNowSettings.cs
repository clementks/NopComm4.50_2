﻿using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.PayNow
{
    /// <summary>
    /// Represents settings of PayNow payment plugin
    /// </summary>
    public class PayNowSettings : ISettings
    {
        /// <summary>
        /// Gets or sets PayNow payment transaction mode
        /// </summary>
        public TransactMode TransactMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }

        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }
    }
}
