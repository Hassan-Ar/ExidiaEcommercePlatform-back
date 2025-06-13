using EcommercePlatform.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Orders.Dtos;

public class OrderDto : AuditedEntityDto<Guid>
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public string PaymentStatus { get; set; }
    public string TrackingNumber { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
} 