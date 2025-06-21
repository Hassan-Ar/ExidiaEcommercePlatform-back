using System;
using System.Collections.Generic;
using EcommercePlatform.Categories;
using EcommercePlatform.Orders;
using EcommercePlatform.Shops;
using EcommercePlatform.DynamicPricing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Products;

public class Product : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
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
    public Category Category { get; set; }
    public Guid ShopId { get; set; }
    public Shop Shop { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<DynamicPriceRule> DynamicPriceRules { get; set; }

    protected Product()
    {
        OrderItems = new List<OrderItem>();
        DynamicPriceRules = new List<DynamicPriceRule>();
    }

    public Product(
        Guid id,
        string name,
        string description,
        decimal price,
        int stockQuantity,
        string sku,
        string imageUrl,
        Guid categoryId,
        Guid shopId,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        SKU = sku;
        ImageUrl = imageUrl;
        IsActive = true;
        DiscountPercent = 0;
        Rating = 0;
        RatingCount = 0;
        CategoryId = categoryId;
        ShopId = shopId;
        OrderItems = new List<OrderItem>();
        DynamicPriceRules = new List<DynamicPriceRule>();
    }

    public void UpdateStock(int quantity)
    {
        if (StockQuantity + quantity < 0)
        {
            throw new InvalidOperationException("Insufficient stock");
        }
        StockQuantity += quantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentException("Price cannot be negative", nameof(newPrice));
        }
        Price = newPrice;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
} 