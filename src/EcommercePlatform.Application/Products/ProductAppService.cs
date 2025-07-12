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
using EcommercePlatform.Shops;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatform.Products;

[AllowAnonymous]
public class ProductAppService :
    CrudAppService<
        Product,
        ProductDto,
        Guid,
        EcommercePlatform.Products.Dtos.GetProductListInput,
        CreateUpdateProductDto>,
    IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IRepository<EcommercePlatform.Shops.Shop, Guid> _shopRepository;

    public ProductAppService(
        IRepository<Product, Guid> repository,
        IBlobStorageService blobStorageService,
        IRepository<EcommercePlatform.Shops.Shop, Guid> shopRepository)
        : base(repository)
    {
        _productRepository = repository;
        _blobStorageService = blobStorageService;
        _shopRepository = shopRepository;
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

        var product =  MapToEntity(input);
  
        // If ShopId is not supplied (default Guid), assign the single existing shop automatically
        if (product.ShopId == Guid.Empty)
        {
            var defaultShop = await _shopRepository.FirstOrDefaultAsync();
            if (defaultShop != null)
            {
                product.ShopId = defaultShop.Id;
            }
        }

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
        // Retrieve existing entity to avoid concurrency issues
        var product = await _productRepository.GetAsync(id);

        if (product == null)
        {
            throw new Volo.Abp.AbpException($"Product with id {id} not found");
        }

        // Handle image update if a new image was provided
        if (input.Image != null)
        {
            // Delete previous image if present
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImageName = product.ImageUrl.Split('/').Last();
                await _blobStorageService.DeleteImageAsync(oldImageName);
            }

            var imageName = await _blobStorageService.SaveImageAsync(input.Image);
            product.ImageUrl = _blobStorageService.GetImageUrl(imageName);
        }

        // Map remaining scalar fields from input onto the existing entity
        MapToEntity(input, product);

        // Ensure ShopId is set: if it became an empty guid (because DTO no longer carries it), assign default shop
        if (product.ShopId == Guid.Empty)
        {
            var defaultShop = await _shopRepository.FirstOrDefaultAsync();
            if (defaultShop != null)
            {
                product.ShopId = defaultShop.Id;
            }
        }

        await _productRepository.UpdateAsync(product, autoSave: true);

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

    public override async Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListInput input)
    {
        var query = await _productRepository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(p => p.Name.Contains(input.Filter) || p.Description.Contains(input.Filter) || p.SKU.Contains(input.Filter));
        }

        if (input.CategoryId.HasValue && input.CategoryId != Guid.Empty)
        {
            query = query.Where(p => p.CategoryId == input.CategoryId.Value);
        }

        if (input.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == input.IsActive.Value);
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            query = ApplySorting(query, input);
        }
        else
        {
            query = query.OrderBy(p => p.Name);
        }

        var totalCount = query.Count();
        var items = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        var dtos = ObjectMapper.Map<List<Product>, List<ProductDto>>(items);

        return new PagedResultDto<ProductDto>(totalCount, dtos);
    }

    /// <summary>
    /// Returns a collection of active products ordered by creation time (newest first).
    /// Intended to be consumed by the public storefront home-page as "featured" items.
    /// </summary>
    public async Task<List<ProductDto>> GetFeaturedAsync(int maxCount = 8)
    {
        if (maxCount <= 0)
        {
            return new List<ProductDto>();
        }

        // Build query
        var queryable = await _productRepository.GetQueryableAsync();

        var products = queryable
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreationTime)
            .Take(maxCount)
            .ToList();

        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }

    /// <inheritdoc />
    public async Task<ProductDto> RateAsync(Guid id, int stars)
    {
        if (stars < 1 || stars > 5)
        {
            throw new ArgumentException("Stars must be between 1 and 5", nameof(stars));
        }

        var product = await _productRepository.GetAsync(id);

        // update average rating with new rating
        product.Rating = ((product.Rating * product.RatingCount) + stars) / (product.RatingCount + 1);
        product.RatingCount += 1;

        await _productRepository.UpdateAsync(product);

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    /// <inheritdoc />
    public async Task<List<ProductDto>> GetLatestDiscountedAsync(int maxCount = 10)
    {
        if (maxCount <= 0)
        {
            return new List<ProductDto>();
        }

        var queryable = await _productRepository.GetQueryableAsync();

        var products = queryable
            .Where(p => p.IsActive && p.DiscountPercent > 0)
            .OrderByDescending(p => p.CreationTime)
            .Take(maxCount)
            .ToList();

        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }
} 