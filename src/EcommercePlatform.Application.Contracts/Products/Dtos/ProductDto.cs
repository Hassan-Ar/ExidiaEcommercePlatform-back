using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Products.Dtos;

public class ProductDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string SKU { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public decimal DiscountPercent { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ShopId { get; set; }
} 