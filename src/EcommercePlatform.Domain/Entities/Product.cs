using System;
using System.Collections.Generic;
using EcommercePlatform.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents a product in the e-commerce platform.
    /// </summary>
    public class Product : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this product belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The base price of the product.
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// The current stock quantity of the product.
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Indicates whether the product is published and visible to customers.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// The ID of the category that this product belongs to.
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// The type of the product.
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// The SKU (Stock Keeping Unit) of the product.
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// The weight of the product (in grams).
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// The dimensions of the product (in JSON format).
        /// </summary>
        public string Dimensions { get; set; }

        /// <summary>
        /// The list of tags associated with the product.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Creates a new Product instance.
        /// </summary>
        protected Product()
        {
        }

        /// <summary>
        /// Creates a new Product instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the product.</param>
        /// <param name="tenantId">The tenant ID that this product belongs to.</param>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="basePrice">The base price of the product.</param>
        /// <param name="categoryId">The ID of the category that this product belongs to.</param>
        /// <param name="productType">The type of the product.</param>
        /// <param name="sku">The SKU of the product.</param>
        public Product(
            Guid id,
            Guid? tenantId,
            string name,
            string description,
            decimal basePrice,
            Guid categoryId,
            ProductType productType = ProductType.Physical,
            string sku = null)
            : base(id)
        {
            TenantId = tenantId;
            Name = name;
            Description = description;
            BasePrice = basePrice;
            CategoryId = categoryId;
            ProductType = productType;
            Sku = sku;
            StockQuantity = 0;
            IsPublished = false;
        }

        /// <summary>
        /// Updates the stock quantity of the product.
        /// </summary>
        /// <param name="newStockQuantity">The new stock quantity.</param>
        public void UpdateStock(int newStockQuantity)
        {
            if (newStockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.", nameof(newStockQuantity));
            }

            StockQuantity = newStockQuantity;
        }

        /// <summary>
        /// Publishes the product, making it visible to customers.
        /// </summary>
        public void Publish()
        {
            IsPublished = true;
        }

        /// <summary>
        /// Unpublishes the product, making it invisible to customers.
        /// </summary>
        public void Unpublish()
        {
            IsPublished = false;
        }

        /// <summary>
        /// Updates the base price of the product.
        /// </summary>
        /// <param name="newBasePrice">The new base price.</param>
        public void UpdateBasePrice(decimal newBasePrice)
        {
            if (newBasePrice < 0)
            {
                throw new ArgumentException("Base price cannot be negative.", nameof(newBasePrice));
            }

            BasePrice = newBasePrice;
        }
    }
}
