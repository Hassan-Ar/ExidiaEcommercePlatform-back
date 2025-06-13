using System;
using Volo.Abp.Domain.Entities;

namespace EcommercePlatform.Carts;

public class CartItem : Entity<Guid>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductSku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation property
    public Cart Cart { get; set; }
} 