using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EcommercePlatform.Shops.Dtos;

namespace EcommercePlatform.Shops;

public class ShopAppService :
    CrudAppService<
        Shop,
        ShopDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateShopDto>,
    IShopAppService
{
    public ShopAppService(IRepository<Shop, Guid> repository) : base(repository)
    {
    }
} 