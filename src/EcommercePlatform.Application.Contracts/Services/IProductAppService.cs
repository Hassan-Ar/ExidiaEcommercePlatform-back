using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EcommercePlatform.Enums;

namespace EcommercePlatform.Application.Contracts.Services
{
    /// <summary>
    /// Interface for the product application service.
    /// </summary>
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
        /// Gets a product by ID.
        /// </summary>
        Task<ProductDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a list of all products.
        /// </summary>
        Task<List<ProductDto>> GetListAsync();

        /// <summary>
        /// Gets a paged list of products.
        /// </summary>
        Task<PagedResultDto<ProductDto>> GetPagedListAsync(PagedAndSortedResultRequestDto input);

        /// <summary>
        /// Gets a list of products by category.
        /// </summary>
        Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId);

        /// <summary>
        /// Creates a new product.
        /// </summary>
        Task<ProductDto> CreateAsync(CreateProductDto input);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input);

        /// <summary>
        /// Deletes a product.
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Publishes a product, making it visible to customers.
        /// </summary>
        Task<ProductDto> PublishAsync(Guid id);

        /// <summary>
        /// Unpublishes a product, making it invisible to customers.
        /// </summary>
        Task<ProductDto> UnpublishAsync(Guid id);

        /// <summary>
        /// Updates the stock quantity of a product.
        /// </summary>
        Task<ProductDto> UpdateStockAsync(Guid id, UpdateProductStockDto input);
    }

    /// <summary>
    /// DTO for product data.
    /// </summary>
    public class ProductDto : EntityDto<Guid>
    {
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
        /// The dynamically calculated price of the product.
        /// </summary>
        public decimal DynamicPrice { get; set; }

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
        /// The creation time of the product.
        /// </summary>
        public DateTime CreationTime { get; set; }
    }

    /// <summary>
    /// DTO for creating a product.
    /// </summary>
    public class CreateProductDto
    {
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
        /// The ID of the category that this product belongs to.
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// The type of the product.
        /// </summary>
        public ProductType ProductType { get; set; } = ProductType.Physical;

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
    }

    /// <summary>
    /// DTO for updating a product.
    /// </summary>
    public class UpdateProductDto
    {
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
        /// The ID of the category that this product belongs to.
        /// </summary>
        public Guid? CategoryId { get; set; }

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
    }

    /// <summary>
    /// DTO for updating product stock.
    /// </summary>
    public class UpdateProductStockDto
    {
        /// <summary>
        /// The new stock quantity.
        /// </summary>
        public int StockQuantity { get; set; }
    }
} 