using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EcommercePlatform.Carts.Dtos;
using EcommercePlatform.Products;

namespace EcommercePlatform.Carts;

public class CartAppService :
    CrudAppService<
        Cart,
        CartDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCartDto>,
    ICartAppService
{
    private readonly IRepository<Cart, Guid> _cartRepository;
    private readonly IRepository<CartItem, Guid> _cartItemRepository;
    private readonly IRepository<Product, Guid> _productRepository;

    public CartAppService(
        IRepository<Cart, Guid> cartRepository,
        IRepository<CartItem, Guid> cartItemRepository,
        IRepository<Product, Guid> productRepository)
        : base(cartRepository)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto> GetCartByUserIdAsync(Guid userId)
    {
        var cart = await _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _cartRepository.InsertAsync(cart);
        }

        await _cartRepository.EnsureCollectionLoadedAsync(cart, c => c.Items);

        return ObjectMapper.Map<Cart, CartDto>(cart);
    }

    public async Task<CartDto> AddItemToCartAsync(Guid cartId, CreateUpdateCartItemDto input)
    {
        var cart = (await Repository.WithDetailsAsync(x => x.Items))
            
            .FirstOrDefault(c => c.Id == cartId);

        if (cart == null)
        {
            throw new Exception($"Cart with id {cartId} not found");
        }

        var product = await _productRepository.GetAsync(input.ProductId);

        var existingItem = cart.Items.FirstOrDefault(ci => ci.ProductId == input.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += input.Quantity;
            existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            await _cartItemRepository.UpdateAsync(existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cartId,
                ProductId = input.ProductId,
                ProductName = product.Name,
                ProductSku = product.SKU,
                Quantity = input.Quantity,
                UnitPrice = product.Price,
                TotalPrice = product.Price * input.Quantity
            };
            await _cartItemRepository.InsertAsync(cartItem);
            cart.Items.Add(cartItem);
        }

        cart.TotalPrice = cart.Items.Sum(ci => ci.TotalPrice);
        await _cartRepository.UpdateAsync(cart);

        return ObjectMapper.Map<Cart, CartDto>(cart);
    }

    public async Task<CartDto> UpdateCartItemQuantityAsync(Guid cartItemId, int quantity)
    {
        var cartItem = await _cartItemRepository.GetAsync(cartItemId);
        cartItem.Quantity = quantity;
        cartItem.TotalPrice = cartItem.Quantity * cartItem.UnitPrice;
        await _cartItemRepository.UpdateAsync(cartItem);

        var cart = (await Repository.WithDetailsAsync(x => x.Items))
            .FirstOrDefault(c => c.Id == cartItem.CartId);
        cart.TotalPrice = cart.Items.Sum(ci => ci.TotalPrice);
        await _cartRepository.UpdateAsync(cart);

        return ObjectMapper.Map<Cart, CartDto>(cart);
    }

    public async Task<CartDto> RemoveItemFromCartAsync(Guid cartItemId)
    {
        var cartItem = await _cartItemRepository.GetAsync(cartItemId);
        var cartId = cartItem.CartId;
        await _cartItemRepository.DeleteAsync(cartItemId);

        var cart = (await Repository.WithDetailsAsync(x => x.Items))
            .FirstOrDefault(c => c.Id == cartId);
        cart.TotalPrice = cart.Items.Sum(ci => ci.TotalPrice);
        await _cartRepository.UpdateAsync(cart);

        return ObjectMapper.Map<Cart, CartDto>(cart);
    }

    public async Task<CartDto> ClearCartAsync(Guid cartId)
    {
        var cart = (await Repository.WithDetailsAsync(x=>x.Items))
            .FirstOrDefault(c => c.Id == cartId);

        if (cart == null)
        {
            throw new Exception($"Cart with id {cartId} not found");
        }

        foreach (var item in cart.Items.ToList())
        {
            await _cartItemRepository.DeleteAsync(item);
        }
        cart.Items.Clear();
        cart.TotalPrice = 0;
        await _cartRepository.UpdateAsync(cart);

        return ObjectMapper.Map<Cart, CartDto>(cart);
    }
} 