using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Carts.Dtos;

public class CartItemDto : EntityDto<Guid>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductSku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
} 