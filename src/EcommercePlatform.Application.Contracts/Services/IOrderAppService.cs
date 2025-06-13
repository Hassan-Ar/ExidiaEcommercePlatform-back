using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EcommercePlatform.Enums;

namespace EcommercePlatform.Application.Contracts.Services
{
    /// <summary>
    /// Interface for the order application service.
    /// </summary>
    public interface IOrderAppService : IApplicationService
    {
        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        Task<OrderDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a list of all orders.
        /// </summary>
        Task<List<OrderDto>> GetListAsync();

        /// <summary>
        /// Gets a paged list of orders.
        /// </summary>
        Task<PagedResultDto<OrderDto>> GetPagedListAsync(PagedAndSortedResultRequestDto input);

        /// <summary>
        /// Gets a list of orders by customer.
        /// </summary>
        Task<List<OrderDto>> GetByCustomerAsync(Guid customerId);

        /// <summary>
        /// Creates a new order.
        /// </summary>
        Task<OrderDto> CreateAsync(CreateOrderDto input);

        /// <summary>
        /// Updates the order status.
        /// </summary>
        Task<OrderDto> UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDto input);

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        Task<OrderDto> UpdatePaymentStatusAsync(Guid id, UpdatePaymentStatusDto input);

        /// <summary>
        /// Marks an order as shipped.
        /// </summary>
        Task<OrderDto> MarkAsShippedAsync(Guid id, ShippingUpdateDto input);

        /// <summary>
        /// Marks an order as delivered.
        /// </summary>
        Task<OrderDto> MarkAsDeliveredAsync(Guid id);

        /// <summary>
        /// Marks an order as completed.
        /// </summary>
        Task<OrderDto> MarkAsCompletedAsync(Guid id);

        /// <summary>
        /// Cancels an order.
        /// </summary>
        Task<OrderDto> CancelOrderAsync(Guid id, CancelOrderDto input);
    }

    /// <summary>
    /// DTO for order data.
    /// </summary>
    public class OrderDto : EntityDto<Guid>
    {
        /// <summary>
        /// The order number.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The ID of the customer who placed the order.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The current status of the order.
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// The subtotal amount of the order (before tax and shipping).
        /// </summary>
        public decimal SubtotalAmount { get; set; }

        /// <summary>
        /// The tax amount applied to the order.
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// The shipping amount for the order.
        /// </summary>
        public decimal ShippingAmount { get; set; }

        /// <summary>
        /// The discount amount applied to the order.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// The payment status of the order.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// The payment method used for the order.
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// The transaction ID from the payment gateway.
        /// </summary>
        public string PaymentTransactionId { get; set; }

        /// <summary>
        /// The shipping address for the order.
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// The billing address for the order.
        /// </summary>
        public string BillingAddress { get; set; }

        /// <summary>
        /// The shipping method used for the order.
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// The tracking number for the shipment.
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Any notes from the customer.
        /// </summary>
        public string CustomerNotes { get; set; }

        /// <summary>
        /// Any notes from the admin.
        /// </summary>
        public string AdminNotes { get; set; }

        /// <summary>
        /// The creation time of the order.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// The list of items in the order.
        /// </summary>
        public List<OrderItemDto> Items { get; set; }
    }

    /// <summary>
    /// DTO for order item data.
    /// </summary>
    public class OrderItemDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the order this item belongs to.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// The ID of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The SKU of the product.
        /// </summary>
        public string ProductSku { get; set; }

        /// <summary>
        /// The quantity of the product in the order.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price of the product at the time of order.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// The total price for this order item.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The discount amount applied to this item.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Any options selected for the product (e.g., size, color).
        /// </summary>
        public string ItemOptions { get; set; }
    }

    /// <summary>
    /// DTO for creating an order.
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// The ID of the customer placing the order.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The list of items to be included in the order.
        /// </summary>
        public List<CreateOrderItemDto> Items { get; set; }

        /// <summary>
        /// The shipping address for the order.
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// The billing address for the order.
        /// </summary>
        public string BillingAddress { get; set; }

        /// <summary>
        /// Any notes from the customer regarding the order.
        /// </summary>
        public string CustomerNotes { get; set; }
    }

    /// <summary>
    /// DTO for creating an order item.
    /// </summary>
    public class CreateOrderItemDto
    {
        /// <summary>
        /// The ID of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The discount amount applied to this item.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Any options selected for the product (e.g., size, color).
        /// </summary>
        public string ItemOptions { get; set; }
    }

    /// <summary>
    /// DTO for updating order status.
    /// </summary>
    public class UpdateOrderStatusDto
    {
        /// <summary>
        /// The new status of the order.
        /// </summary>
        public OrderStatus Status { get; set; }
    }

    /// <summary>
    /// DTO for updating payment status.
    /// </summary>
    public class UpdatePaymentStatusDto
    {
        /// <summary>
        /// The new payment status.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// The payment method used.
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// The transaction ID.
        /// </summary>
        public string TransactionId { get; set; }
    }

    /// <summary>
    /// DTO for shipping updates.
    /// </summary>
    public class ShippingUpdateDto
    {
        /// <summary>
        /// The shipping method used.
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// The tracking number for the shipment.
        /// </summary>
        public string TrackingNumber { get; set; }
    }

    /// <summary>
    /// DTO for canceling an order.
    /// </summary>
    public class CancelOrderDto
    {
        /// <summary>
        /// Notes regarding the cancellation.
        /// </summary>
        public string Notes { get; set; }
    }
} 