using System;

namespace EcommercePlatform.Enums
{
    /// <summary>
    /// Represents the status of an order in the system.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order has been created but not yet processed.
        /// </summary>
        Pending = 0,
        
        /// <summary>
        /// Payment has been received and order is being processed.
        /// </summary>
        Processing = 1,
        
        /// <summary>
        /// Order has been shipped to the customer.
        /// </summary>
        Shipped = 2,
        
        /// <summary>
        /// Order has been delivered to the customer.
        /// </summary>
        Delivered = 3,
        
        /// <summary>
        /// Order has been completed.
        /// </summary>
        Completed = 4,
        
        /// <summary>
        /// Order has been cancelled.
        /// </summary>
        Cancelled = 5,
        
        /// <summary>
        /// Order has been returned by the customer.
        /// </summary>
        Returned = 6
    }
}
