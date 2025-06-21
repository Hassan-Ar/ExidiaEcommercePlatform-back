﻿using AutoMapper;
using EcommercePlatform.AiChat;
using EcommercePlatform.AiChat.Dtos;
using EcommercePlatform.Carts;
using EcommercePlatform.Carts.Dtos;
using EcommercePlatform.Categories;
using EcommercePlatform.Categories.Dtos;
using EcommercePlatform.DynamicPricing;
using EcommercePlatform.DynamicPricing.Dtos;
using EcommercePlatform.Orders;
using EcommercePlatform.Orders.Dtos;
using EcommercePlatform.Products;
using EcommercePlatform.Products.Dtos;
using EcommercePlatform.Shops;
using EcommercePlatform.Shops.Dtos;

namespace EcommercePlatform;

public class EcommercePlatformApplicationAutoMapperProfile : Profile
{
    public EcommercePlatformApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        // Product mappings
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        // Category mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateUpdateCategoryDto, Category>();
        CreateMap<Category, CategoryLookupDto>();

        // Order mappings
        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>();
        CreateMap<OrderItem, OrderItemDto>();

        // Shop mappings
        CreateMap<Shop, ShopDto>();
        CreateMap<CreateUpdateShopDto, Shop>();

        // Dynamic Pricing mappings
        CreateMap<DynamicPriceRule, DynamicPriceRuleDto>();
        CreateMap<CreateUpdateDynamicPriceRuleDto, DynamicPriceRule>();

        // AI Chat mappings
        CreateMap<AiChatSession, AiChatSessionDto>();
        CreateMap<CreateUpdateAiChatSessionDto, AiChatSession>();
        CreateMap<AiChatMessage, AiChatMessageDto>();
        CreateMap<CreateAiChatMessageDto, AiChatMessage>();

        // Cart mappings
        CreateMap<Cart, CartDto>();
        CreateMap<CreateUpdateCartDto, Cart>();
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CreateUpdateCartItemDto, CartItem>();
    }
}
