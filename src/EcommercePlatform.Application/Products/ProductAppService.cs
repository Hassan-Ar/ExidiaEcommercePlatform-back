using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Products;
using EcommercePlatform.Products.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EcommercePlatform.Products;

[Authorize]
public class ProductAppService :
    CrudAppService<
        Product,
        ProductDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProductDto>,
    IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductAppService(IRepository<Product, Guid> repository)
        : base(repository)
    {
        _productRepository = repository;
    }

    public async Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId)
    {
        var products = await _productRepository.GetListAsync(p => p.CategoryId == categoryId);
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> GetByShopAsync(Guid shopId)
    {
        var products = await _productRepository.GetListAsync(p => p.ShopId == shopId);
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }

    public async Task<ProductDto> UpdateStockAsync(Guid id, int quantity)
    {
        var product = await _productRepository.GetAsync(id);
        product.UpdateStock(quantity);
        await _productRepository.UpdateAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task<ProductDto> UpdatePriceAsync(Guid id, decimal newPrice)
    {
        var product = await _productRepository.GetAsync(id);
        product.UpdatePrice(newPrice);
        await _productRepository.UpdateAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task<ProductDto> ToggleActiveStatusAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        if (product.IsActive)
        {
            product.Deactivate();
        }
        else
        {
            product.Activate();
        }
        await _productRepository.UpdateAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }
} 