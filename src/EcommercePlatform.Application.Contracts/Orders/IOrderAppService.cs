using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Enums;
using EcommercePlatform.Orders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EcommercePlatform.Orders;

public interface IOrderAppService :
    ICrudAppService<
        OrderDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateOrderDto>
{
    Task<List<OrderDto>> GetByCustomerAsync(Guid customerId);
    Task<OrderDto> AddOrderItemAsync(Guid orderId, CreateOrderItemDto input);
    Task<OrderDto> RemoveOrderItemAsync(Guid orderId, Guid orderItemId);
    Task<OrderDto> UpdateOrderStatusAsync(Guid id, OrderStatus status);
    Task<OrderDto> UpdatePaymentStatusAsync(Guid id, string status);
    Task<OrderDto> UpdateTrackingNumberAsync(Guid id, string trackingNumber);
} 