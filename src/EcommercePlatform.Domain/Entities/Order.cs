using System;
using System.Collections.Generic;
using EcommercePlatform.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents an order in the e-commerce platform.
    /// </summary>
    public class Order : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this order belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The unique order number.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The ID of the customer who placed the order.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The current status of the order.
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// The subtotal amount of the order (before tax and shipping).
        /// </summary>
        public decimal SubtotalAmount { get; set; }

        /// <summary>
        /// The tax amount of the order.
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// The shipping amount of the order.
        /// </summary>
        public decimal ShippingAmount { get; set; }

        /// <summary>
        /// The discount amount applied to the order.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// The payment status of the order.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// The payment method used for the order.
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// The payment transaction ID.
        /// </summary>
        public string PaymentTransactionId { get; set; }

        /// <summary>
        /// The shipping address for the order.
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// The billing address for the order.
        /// </summary>
        public string BillingAddress { get; set; }

        /// <summary>
        /// The shipping method used for the order.
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// The tracking number for the shipment.
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Customer notes for the order.
        /// </summary>
        public string CustomerNotes { get; set; }

        /// <summary>
        /// Admin notes for the order.
        /// </summary>
        public string AdminNotes { get; set; }

        /// <summary>
        /// Creates a new Order instance.
        /// </summary>
        protected Order()
        {
        }

        /// <summary>
        /// Creates a new Order instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the order.</param>
        /// <param name="tenantId">The tenant ID that this order belongs to.</param>
        /// <param name="orderNumber">The unique order number.</param>
        /// <param name="customerId">The ID of the customer who placed the order.</param>
        /// <param name="subtotalAmount">The subtotal amount of the order.</param>
        /// <param name="taxAmount">The tax amount of the order.</param>
        /// <param name="shippingAmount">The shipping amount of the order.</param>
        /// <param name="discountAmount">The discount amount applied to the order.</param>
        public Order(
            Guid id,
            Guid? tenantId,
            string orderNumber,
            Guid customerId,
            decimal subtotalAmount,
            decimal taxAmount = 0,
            decimal shippingAmount = 0,
            decimal discountAmount = 0)
            : base(id)
        {
            TenantId = tenantId;
            OrderNumber = orderNumber;
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            SubtotalAmount = subtotalAmount;
            TaxAmount = taxAmount;
            ShippingAmount = shippingAmount;
            DiscountAmount = discountAmount;
            TotalAmount = CalculateTotalAmount(subtotalAmount, taxAmount, shippingAmount, discountAmount);
            PaymentStatus = PaymentStatus.Pending;
        }

        /// <summary>
        /// Calculates the total amount of the order.
        /// </summary>
        /// <param name="subtotal">The subtotal amount.</param>
        /// <param name="tax">The tax amount.</param>
        /// <param name="shipping">The shipping amount.</param>
        /// <param name="discount">The discount amount.</param>
        /// <returns>The total amount.</returns>
        private decimal CalculateTotalAmount(decimal subtotal, decimal tax, decimal shipping, decimal discount)
        {
            return subtotal + tax + shipping - discount;
        }

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="newStatus">The new order status.</param>
        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        /// <param name="newStatus">The new payment status.</param>
        /// <param name="paymentMethod">The payment method used.</param>
        /// <param name="transactionId">The payment transaction ID.</param>
        public void UpdatePaymentStatus(PaymentStatus newStatus, string paymentMethod = null, string transactionId = null)
        {
            PaymentStatus = newStatus;
            
            if (paymentMethod != null)
            {
                PaymentMethod = paymentMethod;
            }
            
            if (transactionId != null)
            {
                PaymentTransactionId = transactionId;
            }
            
            if (newStatus == PaymentStatus.Completed && Status == OrderStatus.Pending)
            {
                Status = OrderStatus.Processing;
            }
        }

        /// <summary>
        /// Updates the shipping information.
        /// </summary>
        /// <param name="shippingMethod">The shipping method used.</param>
        /// <param name="trackingNumber">The tracking number for the shipment.</param>
        public void UpdateShippingInfo(string shippingMethod, string trackingNumber = null)
        {
            ShippingMethod = shippingMethod;
            TrackingNumber = trackingNumber;
        }

        /// <summary>
        /// Marks the order as shipped.
        /// </summary>
        /// <param name="trackingNumber">The tracking number for the shipment.</param>
        public void MarkAsShipped(string trackingNumber = null)
        {
            if (Status != OrderStatus.Processing && Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException($"Cannot mark order as shipped. Current status: {Status}");
            }

            Status = OrderStatus.Shipped;
            
            if (trackingNumber != null)
            {
                TrackingNumber = trackingNumber;
            }
        }

        /// <summary>
        /// Marks the order as delivered.
        /// </summary>
        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
            {
                throw new InvalidOperationException($"Cannot mark order as delivered. Current status: {Status}");
            }

            Status = OrderStatus.Delivered;
        }

        /// <summary>
        /// Marks the order as completed.
        /// </summary>
        public void MarkAsCompleted()
        {
            if (Status != OrderStatus.Delivered && Status != OrderStatus.Shipped)
            {
                throw new InvalidOperationException($"Cannot mark order as completed. Current status: {Status}");
            }

            Status = OrderStatus.Completed;
        }

        /// <summary>
        /// Cancels the order.
        /// </summary>
        /// <param name="notes">Notes about the cancellation.</param>
        public void Cancel(string notes = null)
        {
            if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered || Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException($"Cannot cancel order. Current status: {Status}");
            }

            Status = OrderStatus.Cancelled;
            
            if (notes != null)
            {
                AdminNotes = notes;
            }
        }
    }
}
