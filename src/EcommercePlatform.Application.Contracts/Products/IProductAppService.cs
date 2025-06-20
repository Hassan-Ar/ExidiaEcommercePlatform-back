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
} 