using System;

namespace EcommercePlatform.Enums
{
    /// <summary>
    /// Represents the type of product in the system.
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// A physical product that requires shipping.
        /// </summary>
        Physical = 0,
        
        /// <summary>
        /// A digital product that can be downloaded.
        /// </summary>
        Digital = 1,
        
        /// <summary>
        /// A service that can be booked or scheduled.
        /// </summary>
        Service = 2,
        
        /// <summary>
        /// A subscription product with recurring billing.
        /// </summary>
        Subscription = 3
    }
}
