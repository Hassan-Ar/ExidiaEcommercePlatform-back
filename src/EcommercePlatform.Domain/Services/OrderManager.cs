using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using EcommercePlatform.Enums;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// Domain service for managing orders.
    /// </summary>
    public class OrderManager : DomainService, IOrderManager
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IRepository<Product, Guid> _productRepository;

        /// <summary>
        /// Creates a new instance of OrderManager.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        /// <param name="orderItemRepository">The order item repository.</param>
        /// <param name="productRepository">The product repository.</param>
        public OrderManager(
            IRepository<Order, Guid> orderRepository,
            IRepository<OrderItem, Guid> orderItemRepository,
            IRepository<Product, Guid> productRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="customerId">The ID of the customer who placed the order.</param>
        /// <param name="items">The items in the order.</param>
        /// <param name="shippingAddress">The shipping address for the order.</param>
        /// <param name="billingAddress">The billing address for the order.</param>
        /// <param name="customerNotes">Customer notes for the order.</param>
        /// <returns>The newly created order.</returns>
        public async Task<Order> CreateOrderAsync(
            Guid customerId,
            List<OrderItemManagerDto> items,
            string shippingAddress,
            string billingAddress = null,
            string customerNotes = null)
        {
            // Validate inputs
            if (customerId == Guid.Empty)
            {
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
            }

            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("Order must have at least one item.", nameof(items));
            }

            if (string.IsNullOrWhiteSpace(shippingAddress))
            {
                throw new ArgumentException("Shipping address cannot be empty.", nameof(shippingAddress));
            }

            // Generate order number
            var orderNumber = await GenerateOrderNumberAsync();

            // Calculate order amounts
            decimal subtotal = 0;
            decimal taxAmount = 0;
            decimal shippingAmount = 0;
            decimal discountAmount = 0;

            // Create order
            var order = new Order(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                orderNumber,
                customerId,
                subtotal,
                taxAmount,
                shippingAmount,
                discountAmount);

            order.ShippingAddress = shippingAddress;
            order.BillingAddress = billingAddress ?? shippingAddress;
            order.CustomerNotes = customerNotes;

            // Insert order to get ID
            await _orderRepository.InsertAsync(order);

            // Process order items
            foreach (var item in items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);

                // Create order item
                var orderItem = new OrderItem(
                    GuidGenerator.Create(),
                    CurrentTenant.Id,
                    order.Id,
                    product.Id,
                    product.Name,
                    product.Sku,
                    item.Quantity,
                    product.BasePrice,
                    item.DiscountAmount,
                    item.ItemOptions);

                await _orderItemRepository.InsertAsync(orderItem);

                // Update subtotal
                subtotal += orderItem.TotalPrice;

                // Update product stock
                product.UpdateStock(product.StockQuantity - item.Quantity);
                await _productRepository.UpdateAsync(product);
            }

            // Calculate tax (simplified example: 10% of subtotal)
            taxAmount = subtotal * 0.1m;

            // Calculate shipping (simplified example: flat rate)
            shippingAmount = 10.0m;

            // Update order with final amounts
            order.SubtotalAmount = subtotal;
            order.TaxAmount = taxAmount;
            order.ShippingAmount = shippingAmount;
            order.DiscountAmount = discountAmount;
            order.TotalAmount = subtotal + taxAmount + shippingAmount - discountAmount;

            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="status">The new order status.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> UpdateOrderStatusAsync(Guid id, OrderStatus status)
        {
            var order = await _orderRepository.GetAsync(id);
            order.UpdateStatus(status);
            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="paymentStatus">The new payment status.</param>
        /// <param name="paymentMethod">The payment method used.</param>
        /// <param name="transactionId">The payment transaction ID.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> UpdatePaymentStatusAsync(
            Guid id,
            PaymentStatus paymentStatus,
            string paymentMethod = null,
            string transactionId = null)
        {
            var order = await _orderRepository.GetAsync(id);
            order.UpdatePaymentStatus(paymentStatus, paymentMethod, transactionId);
            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Marks an order as shipped.
        /// </summary>
        /// <param name="id">The ID of the order to mark as shipped.</param>
        /// <param name="trackingNumber">The tracking number for the shipment.</param>
        /// <param name="shippingMethod">The shipping method used.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> MarkAsShippedAsync(
            Guid id,
            string trackingNumber = null,
            string shippingMethod = null)
        {
            var order = await _orderRepository.GetAsync(id);
            
            if (!string.IsNullOrWhiteSpace(shippingMethod))
            {
                order.UpdateShippingInfo(shippingMethod, trackingNumber);
            }
            
            order.MarkAsShipped(trackingNumber);
            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Marks an order as delivered.
        /// </summary>
        /// <param name="id">The ID of the order to mark as delivered.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> MarkAsDeliveredAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            order.MarkAsDelivered();
            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Marks an order as completed.
        /// </summary>
        /// <param name="id">The ID of the order to mark as completed.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> MarkAsCompletedAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            order.MarkAsCompleted();
            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <param name="notes">Notes about the cancellation.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> CancelOrderAsync(Guid id, string notes = null)
        {
            var order = await _orderRepository.GetAsync(id);
            order.Cancel(notes);
            
            // Restore product stock
            var orderItems = await _orderItemRepository.GetListAsync(oi => oi.OrderId == id);
            foreach (var item in orderItems)
            {
                var product = await _productRepository.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.UpdateStock(product.StockQuantity + item.Quantity);
                    await _productRepository.UpdateAsync(product);
                }
            }
            
            return await _orderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// Generates a unique order number.
        /// </summary>
        /// <returns>A unique order number.</returns>
        private async Task<string> GenerateOrderNumberAsync()
        {
            var prefix = DateTime.UtcNow.ToString("yyyyMMdd");
            var random = new Random();
            var suffix = random.Next(1000, 9999).ToString();
            
            var orderNumber = $"ORD-{prefix}-{suffix}";
            
            // Ensure order number is unique
            while (await _orderRepository.AnyAsync(o => o.OrderNumber == orderNumber))
            {
                suffix = random.Next(1000, 9999).ToString();
                orderNumber = $"ORD-{prefix}-{suffix}";
            }
            
            return orderNumber;
        }
    }

    /// <summary>
    /// Interface for the order manager domain service.
    /// </summary>
    public interface IOrderManager : IDomainService
    {
        /// <summary>
        /// Creates a new order.
        /// </summary>
        Task<Order> CreateOrderAsync(
            Guid customerId,
            List<OrderItemManagerDto> items,
            string shippingAddress,
            string billingAddress = null,
            string customerNotes = null);

        /// <summary>
        /// Updates the order status.
        /// </summary>
        Task<Order> UpdateOrderStatusAsync(Guid id, OrderStatus status);

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        Task<Order> UpdatePaymentStatusAsync(
            Guid id,
            PaymentStatus paymentStatus,
            string paymentMethod = null,
            string transactionId = null);

        /// <summary>
        /// Marks an order as shipped.
        /// </summary>
        Task<Order> MarkAsShippedAsync(
            Guid id,
            string trackingNumber = null,
            string shippingMethod = null);

        /// <summary>
        /// Marks an order as delivered.
        /// </summary>
        Task<Order> MarkAsDeliveredAsync(Guid id);

        /// <summary>
        /// Marks an order as completed.
        /// </summary>
        Task<Order> MarkAsCompletedAsync(Guid id);

        /// <summary>
        /// Cancels an order.
        /// </summary>
        Task<Order> CancelOrderAsync(Guid id, string notes = null);
    }

    /// <summary>
    /// Data transfer object for order items.
    /// </summary>
    public class OrderItemManagerDto
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
        /// Any discount applied to this item.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Any additional options or attributes selected for this item (in JSON format).
        /// </summary>
        public string ItemOptions { get; set; }
    }
}
