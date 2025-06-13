using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Carts.Dtos;

public class CartDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItemDto> Items { get; set; }
} 