using System;

namespace EcommercePlatform.Enums
{
    /// <summary>
    /// Represents the status of a payment in the system.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment is pending.
        /// </summary>
        Pending = 0,
        
        /// <summary>
        /// Payment has been authorized but not captured.
        /// </summary>
        Authorized = 1,
        
        /// <summary>
        /// Payment has been captured successfully.
        /// </summary>
        Completed = 2,
        
        /// <summary>
        /// Payment has been refunded.
        /// </summary>
        Refunded = 3,
        
        /// <summary>
        /// Payment has failed.
        /// </summary>
        Failed = 4,
        
        /// <summary>
        /// Payment has been cancelled.
        /// </summary>
        Cancelled = 5
    }
}
