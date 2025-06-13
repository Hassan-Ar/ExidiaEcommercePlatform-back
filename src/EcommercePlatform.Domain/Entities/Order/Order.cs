using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Orders;

public class Order : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public string OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string Notes { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }

    protected Order()
    {
        OrderItems = new List<OrderItem>();
    }

    public Order(
        Guid id,
        Guid customerId,
        string orderNumber,
        string shippingAddress,
        string billingAddress,
        string paymentMethod,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        CustomerId = customerId;
        OrderNumber = orderNumber;
        Status = OrderStatus.Pending;
        TotalAmount = 0;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        PaymentMethod = paymentMethod;
        PaymentStatus = "Pending";
        OrderItems = new List<OrderItem>();
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        OrderItems.Add(orderItem);
        RecalculateTotalAmount();
    }

    public void RemoveOrderItem(OrderItem orderItem)
    {
        OrderItems.Remove(orderItem);
        RecalculateTotalAmount();
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        
        switch (newStatus)
        {
            case OrderStatus.Shipped:
                ShippedDate = DateTime.UtcNow;
                break;
            case OrderStatus.Delivered:
                DeliveredDate = DateTime.UtcNow;
                break;
        }
    }

    public void UpdatePaymentStatus(string status)
    {
        PaymentStatus = status;
        if (status == "Paid")
        {
            PaymentDate = DateTime.UtcNow;
        }
    }

    public void UpdateTrackingNumber(string trackingNumber)
    {
        TrackingNumber = trackingNumber;
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = OrderItems.Sum(item => item.TotalPrice);
    }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
} 