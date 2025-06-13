using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Shops.Dtos;

public class ShopDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string SellerId { get; set; }
    public bool IsActive { get; set; }
} 