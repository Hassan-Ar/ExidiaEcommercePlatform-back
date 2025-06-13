using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Entities;
using EcommercePlatform.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// Domain service for managing products.
    /// </summary>
    public class ProductManager : DomainService, IProductManager
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;

        /// <summary>
        /// Creates a new instance of ProductManager.
        /// </summary>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="categoryRepository">The category repository.</param>
        public ProductManager(
            IRepository<Product, Guid> productRepository,
            IRepository<Category, Guid> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="basePrice">The base price of the product.</param>
        /// <param name="categoryId">The ID of the category that this product belongs to.</param>
        /// <param name="productType">The type of the product.</param>
        /// <param name="sku">The SKU of the product.</param>
        /// <returns>The newly created product.</returns>
        public async Task<Product> CreateAsync(
            string name,
            string description,
            decimal basePrice,
            Guid categoryId,
            ProductType productType = ProductType.Physical,
            string sku = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }

            if (basePrice < 0)
            {
                throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));
            }

            // Ensure category exists
            if (!await _categoryRepository.AnyAsync(c => c.Id == categoryId))
            {
                throw new ArgumentException("Category does not exist.", nameof(categoryId));
            }

            // Generate SKU if not provided
            if (string.IsNullOrWhiteSpace(sku))
            {
                sku = await GenerateSkuAsync(name);
            }

            // Create product
            var product = new Product(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                name,
                description,
                basePrice,
                categoryId,
                productType,
                sku);

            return await _productRepository.InsertAsync(product);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="name">The new name of the product.</param>
        /// <param name="description">The new description of the product.</param>
        /// <param name="basePrice">The new base price of the product.</param>
        /// <param name="categoryId">The new category ID of the product.</param>
        /// <returns>The updated product.</returns>
        public async Task<Product> UpdateAsync(
            Guid id,
            string name,
            string description,
            decimal basePrice,
            Guid? categoryId = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }

            if (basePrice < 0)
            {
                throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));
            }

            // Get product
            var product = await _productRepository.GetAsync(id);

            // Update category if provided
            if (categoryId.HasValue && categoryId.Value != product.CategoryId)
            {
                // Ensure category exists
                if (!await _categoryRepository.AnyAsync(c => c.Id == categoryId.Value))
                {
                    throw new ArgumentException("Category does not exist.", nameof(categoryId));
                }

                product.CategoryId = categoryId.Value;
            }

            // Update product properties
            product.Name = name;
            product.Description = description;
            product.UpdateBasePrice(basePrice);

            return await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// Publishes a product, making it visible to customers.
        /// </summary>
        /// <param name="id">The ID of the product to publish.</param>
        /// <returns>The published product.</returns>
        public async Task<Product> PublishAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            product.Publish();
            return await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// Unpublishes a product, making it invisible to customers.
        /// </summary>
        /// <param name="id">The ID of the product to unpublish.</param>
        /// <returns>The unpublished product.</returns>
        public async Task<Product> UnpublishAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            product.Unpublish();
            return await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// Updates the stock quantity of a product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="newStockQuantity">The new stock quantity.</param>
        /// <returns>The updated product.</returns>
        public async Task<Product> UpdateStockAsync(Guid id, int newStockQuantity)
        {
            var product = await _productRepository.GetAsync(id);
            product.UpdateStock(newStockQuantity);
            return await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// Generates a SKU for a product based on its name.
        /// </summary>
        /// <param name="productName">The name of the product.</param>
        /// <returns>A unique SKU.</returns>
        private async Task<string> GenerateSkuAsync(string productName)
        {
            // Generate a SKU based on the product name (first 3 letters) and a random number
            var prefix = productName.Length >= 3 
                ? productName.Substring(0, 3).ToUpper() 
                : productName.PadRight(3, 'X').ToUpper();
            
            var random = new Random();
            var suffix = random.Next(10000, 99999).ToString();
            
            var sku = $"{prefix}-{suffix}";
            
            // Ensure SKU is unique
            while (await _productRepository.AnyAsync(p => p.Sku == sku))
            {
                suffix = random.Next(10000, 99999).ToString();
                sku = $"{prefix}-{suffix}";
            }
            
            return sku;
        }
    }

    /// <summary>
    /// Interface for the product manager domain service.
    /// </summary>
    public interface IProductManager : IDomainService
    {
        /// <summary>
        /// Creates a new product.
        /// </summary>
        Task<Product> CreateAsync(
            string name,
            string description,
            decimal basePrice,
            Guid categoryId,
            ProductType productType = ProductType.Physical,
            string sku = null);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        Task<Product> UpdateAsync(
            Guid id,
            string name,
            string description,
            decimal basePrice,
            Guid? categoryId = null);

        /// <summary>
        /// Publishes a product, making it visible to customers.
        /// </summary>
        Task<Product> PublishAsync(Guid id);

        /// <summary>
        /// Unpublishes a product, making it invisible to customers.
        /// </summary>
        Task<Product> UnpublishAsync(Guid id);

        /// <summary>
        /// Updates the stock quantity of a product.
        /// </summary>
        Task<Product> UpdateStockAsync(Guid id, int newStockQuantity);
    }
}
