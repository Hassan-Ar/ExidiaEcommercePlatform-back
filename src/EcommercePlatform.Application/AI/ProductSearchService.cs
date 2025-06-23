using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.Products.Dtos;
using Volo.Abp.Domain.Repositories;
using EcommercePlatform.Products;

namespace EcommercePlatform.ChatAssistant;

public interface IProductSearchService
{
    Task<List<ProductDto>> SearchProductsAsync(string query, int maxResults = 5);
}

/// <summary>
/// Very simple keyword-based search implementation. For production use, replace with vector search or full-text search.
/// </summary>
public class ProductSearchService : IProductSearchService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductSearchService(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> SearchProductsAsync(string query, int maxResults = 5)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<ProductDto>();
        }

        // Tokenize the query on whitespace and punctuation.
        var tokens = query.Split(new[] {' ', ',', '.', ';', '\\', '/', '-', '_'}, StringSplitOptions.RemoveEmptyEntries)
                          .Select(t => t.Trim())
                          .Where(t => t.Length > 1)
                          .ToArray();

        var allActiveProducts = await _productRepository.GetListAsync(p => p.IsActive);

        // Rank products by the number of tokens that appear in name or description.
        var ranked = allActiveProducts.Select(p => new
        {
            Product = p,
            Score = tokens.Count(tok => (p.Name?.IndexOf(tok, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                                         (p.Description?.IndexOf(tok, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
        })
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Take(maxResults)
        .Select(x => x.Product)
        .ToList();

        // Fallback: if nothing matched, just return latest active products.
        if (!ranked.Any())
        {
            ranked = allActiveProducts.Take(maxResults).ToList();
        }

        return ranked.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            SKU = p.SKU,
            ImageUrl = p.ImageUrl,
            IsActive = p.IsActive,
            DiscountPercent = p.DiscountPercent,
            Rating = p.Rating,
            RatingCount = p.RatingCount,
            CategoryId = p.CategoryId,
            ShopId = p.ShopId
        }).ToList();
    }
} 