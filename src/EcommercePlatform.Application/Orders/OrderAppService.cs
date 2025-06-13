using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.Entities;
using EcommercePlatform.Orders.Dtos;
using EcommercePlatform.Products;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EcommercePlatform.Orders;

public class OrderAppService :
    CrudAppService<
        Order,
        OrderDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateOrderDto>,
    IOrderAppService
{
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<Product, Guid> _productRepository;

    public OrderAppService(
        IRepository<Order, Guid> orderRepository,
        IRepository<Product, Guid> productRepository)
        : base(orderRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public Task<OrderDto> AddOrderItemAsync(Guid orderId, CreateOrderItemDto input)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDto>> GetByCustomerAsync(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> RemoveOrderItemAsync(Guid orderId, Guid orderItemId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> UpdateOrderStatusAsync(Guid id, Enums.OrderStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> UpdatePaymentStatusAsync(Guid id, string status)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> UpdateTrackingNumberAsync(Guid id, string trackingNumber)
    {
        throw new NotImplementedException();
    }

    //public async Task<List<OrderDto>> GetByCustomerAsync(Guid customerId)
    //{
    //    var orders = (await Repository.WithDetailsAsync(x=>x.OrderItems))
    //        .Where(o => o.CustomerId == customerId)
    //        .ToList();

    //    return ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
    //}

    //public async Task<OrderDto> AddOrderItemAsync(Guid orderId, CreateOrderItemDto input)
    //{
    //    var order =  (await Repository.WithDetailsAsync(x => x.OrderItems))
    //        .FirstOrDefault(o => o.Id == orderId);

    //    if (order == null)
    //    {
    //        throw new Exception($"Order with id {orderId} not found");
    //    }

    //    var product = await _productRepository.GetAsync(input.ProductId);
    //    var orderItem = new OrderItem
    //    {
    //        OrderId = orderId,
    //        ProductId = input.ProductId,
    //        ProductName = product.Name,
    //        ProductSku = product.Sku,
    //        Quantity = input.Quantity,
    //        UnitPrice = product.BasePrice,
    //        TotalPrice = product.BasePrice * input.Quantity
    //    };

    //    order.OrderItems.Add(orderItem);
    //    order.TotalAmount = order.OrderItems.Sum(oi => oi.TotalPrice);

    //    await _orderRepository.UpdateAsync(order);

    //    return ObjectMapper.Map<Order, OrderDto>(order);
    //}

    //public async Task<OrderDto> RemoveOrderItemAsync(Guid orderId, Guid orderItemId)
    //{
    //    var order = (await Repository.WithDetailsAsync(x => x.OrderItems))
    //        .FirstOrDefault(o => o.Id == orderId);

    //    if (order == null)
    //    {
    //        throw new Exception($"Order with id {orderId} not found");
    //    }

    //    var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
    //    if (orderItem == null)
    //    {
    //        throw new Exception($"Order item with id {orderItemId} not found");
    //    }

    //    order.OrderItems.Remove(orderItem);
    //    order.TotalAmount = order.OrderItems.Sum(oi => oi.TotalPrice);

    //    await _orderRepository.UpdateAsync(order);

    //    return ObjectMapper.Map<Order, OrderDto>(order);
    //}

    //public async Task<OrderDto> UpdateOrderStatusAsync(Guid id, OrderStatus status)
    //{
    //    var order = await _orderRepository.GetAsync(id);
    //    order.Status = status;
    //    await _orderRepository.UpdateAsync(order);
    //    return ObjectMapper.Map<Order, OrderDto>(order);
    //}

    //public async Task<OrderDto> UpdatePaymentStatusAsync(Guid id, string status)
    //{
    //    var order = await _orderRepository.GetAsync(id);
    //    order.PaymentStatus = status;
    //    await _orderRepository.UpdateAsync(order);
    //    return ObjectMapper.Map<Order, OrderDto>(order);
    //}

    //public async Task<OrderDto> UpdateTrackingNumberAsync(Guid id, string trackingNumber)
    //{
    //    var order = await _orderRepository.GetAsync(id);
    //    order.TrackingNumber = trackingNumber;
    //    await _orderRepository.UpdateAsync(order);
    //    return ObjectMapper.Map<Order, OrderDto>(order);
    //}

    //public Task<OrderDto> UpdateOrderStatusAsync(Guid id, Enums.OrderStatus status)
    //{
    //    throw new NotImplementedException();
    //}
} 