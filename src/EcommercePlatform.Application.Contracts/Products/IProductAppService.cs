using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Products.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EcommercePlatform.Products;

public interface IProductAppService :
    ICrudAppService<
        ProductDto,
        Guid,
        GetProductListInput,
        CreateUpdateProductDto>
{
    Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId);
    Task<List<ProductDto>> GetByShopAsync(Guid shopId);
    Task<ProductDto> UpdateStockAsync(Guid id, int quantity);
    Task<ProductDto> UpdatePriceAsync(Guid id, decimal newPrice);
    Task<ProductDto> ToggleActiveStatusAsync(Guid id);
    /// <summary>
    /// Returns the latest active products to be showcased on the public store home-page.
    /// </summary>
    /// <param name="maxCount">Maximum number of items to return. Defaults to 8.</param>
    /// <returns>A list containing at most <paramref name="maxCount"/> <see cref="ProductDto"/> objects.</returns>
    Task<List<ProductDto>> GetFeaturedAsync(int maxCount = 8);
    /// <summary>
    /// Adds a customer rating (1-5) to the product and returns the updated product.
    /// </summary>
    Task<ProductDto> RateAsync(Guid id, int stars);
    /// <summary>
    /// Returns the latest products that have a non-zero DiscountPercent.
    /// </summary>
    Task<List<ProductDto>> GetLatestDiscountedAsync(int maxCount = 10);
} 