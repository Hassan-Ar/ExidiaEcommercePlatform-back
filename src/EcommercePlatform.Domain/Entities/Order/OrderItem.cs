using System;
using EcommercePlatform.Products;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Orders;

public class OrderItem : FullAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string ProductName { get; set; }
    public string ProductSku { get; set; }

    public OrderItem()
    {
    }

    public OrderItem(
        Guid id,
        Guid orderId,
        Guid productId,
        int quantity,
        decimal unitPrice,
        string productName,
        string productSku,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        ProductName = productName;
        ProductSku = productSku;
        CalculateTotalPrice();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));
        }
        Quantity = newQuantity;
        CalculateTotalPrice();
    }

    public void UpdateUnitPrice(decimal newUnitPrice)
    {
        if (newUnitPrice < 0)
        {
            throw new ArgumentException("Unit price cannot be negative", nameof(newUnitPrice));
        }
        UnitPrice = newUnitPrice;
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = Quantity * UnitPrice;
    }
} 