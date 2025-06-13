using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents an order item in the e-commerce platform.
    /// </summary>
    public class OrderItem : FullAuditedEntity<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this order item belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The ID of the order that this item belongs to.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// The ID of the product that this item represents.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product at the time of order.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The SKU of the product at the time of order.
        /// </summary>
        public string ProductSku { get; set; }

        /// <summary>
        /// The quantity of the product ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price of the product at the time of order.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// The total price for this item (quantity * unit price).
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Any discount applied to this item.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Any additional options or attributes selected for this item (in JSON format).
        /// </summary>
        public string ItemOptions { get; set; }

        /// <summary>
        /// Creates a new OrderItem instance.
        /// </summary>
        protected OrderItem()
        {
        }

        /// <summary>
        /// Creates a new OrderItem instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the order item.</param>
        /// <param name="tenantId">The tenant ID that this order item belongs to.</param>
        /// <param name="orderId">The ID of the order that this item belongs to.</param>
        /// <param name="productId">The ID of the product that this item represents.</param>
        /// <param name="productName">The name of the product at the time of order.</param>
        /// <param name="productSku">The SKU of the product at the time of order.</param>
        /// <param name="quantity">The quantity of the product ordered.</param>
        /// <param name="unitPrice">The unit price of the product at the time of order.</param>
        /// <param name="discountAmount">Any discount applied to this item.</param>
        /// <param name="itemOptions">Any additional options or attributes selected for this item.</param>
        public OrderItem(
            Guid id,
            Guid? tenantId,
            Guid orderId,
            Guid productId,
            string productName,
            string productSku,
            int quantity,
            decimal unitPrice,
            decimal discountAmount = 0,
            string itemOptions = null)
            : base(id)
        {
            TenantId = tenantId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            ProductSku = productSku;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountAmount = discountAmount;
            ItemOptions = itemOptions;
            TotalPrice = CalculateTotalPrice(quantity, unitPrice, discountAmount);
        }

        /// <summary>
        /// Calculates the total price for this item.
        /// </summary>
        /// <param name="quantity">The quantity of the product ordered.</param>
        /// <param name="unitPrice">The unit price of the product.</param>
        /// <param name="discountAmount">Any discount applied to this item.</param>
        /// <returns>The total price for this item.</returns>
        private decimal CalculateTotalPrice(int quantity, decimal unitPrice, decimal discountAmount)
        {
            return (quantity * unitPrice) - discountAmount;
        }

        /// <summary>
        /// Updates the quantity of this item.
        /// </summary>
        /// <param name="newQuantity">The new quantity.</param>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
            }

            Quantity = newQuantity;
            TotalPrice = CalculateTotalPrice(newQuantity, UnitPrice, DiscountAmount);
        }

        /// <summary>
        /// Updates the discount amount for this item.
        /// </summary>
        /// <param name="newDiscountAmount">The new discount amount.</param>
        public void UpdateDiscount(decimal newDiscountAmount)
        {
            if (newDiscountAmount < 0)
            {
                throw new ArgumentException("Discount amount cannot be negative.", nameof(newDiscountAmount));
            }

            if (newDiscountAmount > (Quantity * UnitPrice))
            {
                throw new ArgumentException("Discount amount cannot be greater than the total price.", nameof(newDiscountAmount));
            }

            DiscountAmount = newDiscountAmount;
            TotalPrice = CalculateTotalPrice(Quantity, UnitPrice, newDiscountAmount);
        }
    }
}
