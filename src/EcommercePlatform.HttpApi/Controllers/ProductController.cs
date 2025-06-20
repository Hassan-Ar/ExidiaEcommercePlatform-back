using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Products;
using EcommercePlatform.Products.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Controllers;

[Route("api/app/product")]
public class ProductController : EcommercePlatformController
{
    private readonly IProductAppService _productAppService;

    public ProductController(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListInput input)
    {
        return _productAppService.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<ProductDto> GetAsync(Guid id)
    {
        return _productAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<ProductDto> CreateAsync([FromForm] CreateUpdateProductDto input)
    {
        return _productAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<ProductDto> UpdateAsync(Guid id, [FromForm] CreateUpdateProductDto input)
    {
        return _productAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _productAppService.DeleteAsync(id);
    }

    [HttpGet("by-category/{categoryId}")]
    public virtual Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId)
    {
        return _productAppService.GetByCategoryAsync(categoryId);
    }

    [HttpGet("by-shop/{shopId}")]
    public virtual Task<List<ProductDto>> GetByShopAsync(Guid shopId)
    {
        return _productAppService.GetByShopAsync(shopId);
    }

    [HttpPut("{id}/stock")]
    public virtual Task<ProductDto> UpdateStockAsync(Guid id, [FromBody] int quantity)
    {
        return _productAppService.UpdateStockAsync(id, quantity);
    }

    [HttpPut("{id}/price")]
    public virtual Task<ProductDto> UpdatePriceAsync(Guid id, [FromBody] decimal newPrice)
    {
        return _productAppService.UpdatePriceAsync(id, newPrice);
    }

    [HttpPost("{id}/toggle-active-status")]
    public virtual Task<ProductDto> ToggleActiveStatusAsync(Guid id)
    {
        return _productAppService.ToggleActiveStatusAsync(id);
    }
}