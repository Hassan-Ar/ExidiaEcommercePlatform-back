using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.BlobStoring;
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
    private readonly IBlobStorageService _blobStorageService;

    public ProductAppService(
        IRepository<Product, Guid> repository,
        IBlobStorageService blobStorageService)
        : base(repository)
    {
        _productRepository = repository;
        _blobStorageService = blobStorageService;
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

    public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        if (input == null) return null;

        var product =  MapToEntity(input); //await base.CreateAsync(input);

        if (input.Image != null)
        {
            var imageName = await _blobStorageService.SaveImageAsync(input.Image);
            product.ImageUrl = _blobStorageService.GetImageUrl(imageName);
        }
        await _productRepository.InsertAsync(product,autoSave:true);

        return MapToGetOutputDto(product);
    }

    public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var product = MapToEntity(input);

        if (input.Image != null)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImageName = product.ImageUrl.Split('/').Last();
                await _blobStorageService.DeleteImageAsync(oldImageName);
            }

            // Save new image
            var imageName = await _blobStorageService.SaveImageAsync(input.Image);
            product.ImageUrl = _blobStorageService.GetImageUrl(imageName);
        }
        await _productRepository.UpdateAsync(product);

        return MapToGetOutputDto(product);
    }

    public override async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        
        // Delete image if exists
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            var imageName = product.ImageUrl.Split('/').Last();
            await _blobStorageService.DeleteImageAsync(imageName);
        }

        await base.DeleteAsync(id);
    }
} 