using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EcommercePlatform.Carts.Dtos;

namespace EcommercePlatform.Carts;

public interface ICartAppService :
    ICrudAppService<
        CartDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCartDto>
{
    Task<CartDto> GetCartByUserIdAsync(Guid userId);
    Task<CartDto> AddItemToCartAsync(Guid cartId, CreateUpdateCartItemDto input);
    Task<CartDto> UpdateCartItemQuantityAsync(Guid cartItemId, int quantity);
    Task<CartDto> RemoveItemFromCartAsync(Guid cartItemId);
    Task<CartDto> ClearCartAsync(Guid cartId);
} 