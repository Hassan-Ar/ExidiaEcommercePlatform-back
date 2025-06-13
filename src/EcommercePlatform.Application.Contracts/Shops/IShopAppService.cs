using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EcommercePlatform.Shops.Dtos;

namespace EcommercePlatform.Shops;

public interface IShopAppService :
    ICrudAppService<
        ShopDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateShopDto>
{
    
} 