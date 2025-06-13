using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Application.Services;
using EcommercePlatform.Enums;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace EcommercePlatform.Blazor.Pages.Orders
{
    public partial class OrderList : AbpComponentBase
    {
        [Inject]
        protected IOrderAppService OrderAppService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        private int TotalCount { get; set; }
        private int CurrentPage { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private string SearchTerm { get; set; }
        private OrderStatus? FilterStatus { get; set; }
        private PaymentStatus? FilterPaymentStatus { get; set; }
        private string SortField { get; set; } = "CreationTime";
        private bool SortAscending { get; set; } = false;
        private DateTime? StartDate { get; set; }
        private DateTime? EndDate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            var skipCount = (CurrentPage - 1) * PageSize;
            var sorting = SortAscending ? SortField : SortField + " DESC";

            var input = new PagedAndSortedResultRequestDto
            {
                SkipCount = skipCount,
                MaxResultCount = PageSize,
                Sorting = sorting
            };

            var result = await OrderAppService.GetPagedListAsync(input);
            Orders = result.Items;
            TotalCount = result.TotalCount;

            // Apply client-side filtering if needed
            // In a real implementation, these filters would be passed to the backend
            if (FilterStatus.HasValue || FilterPaymentStatus.HasValue || !string.IsNullOrWhiteSpace(SearchTerm) || StartDate.HasValue || EndDate.HasValue)
            {
                var filteredOrders = new List<OrderDto>();
                
                foreach (var order in Orders)
                {
                    bool includeOrder = true;
                    
                    if (FilterStatus.HasValue && order.Status != FilterStatus.Value)
                    {
                        includeOrder = false;
                    }
                    
                    if (FilterPaymentStatus.HasValue && order.PaymentStatus != FilterPaymentStatus.Value)
                    {
                        includeOrder = false;
                    }
                    
                    if (!string.IsNullOrWhiteSpace(SearchTerm) && 
                        !order.OrderNumber.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        includeOrder = false;
                    }
                    
                    if (StartDate.HasValue && order.CreationTime < StartDate.Value)
                    {
                        includeOrder = false;
                    }
                    
                    if (EndDate.HasValue && order.CreationTime > EndDate.Value.AddDays(1))
                    {
                        includeOrder = false;
                    }
                    
                    if (includeOrder)
                    {
                        filteredOrders.Add(order);
                    }
                }
                
                Orders = filteredOrders;
                TotalCount = filteredOrders.Count;
            }
        }

        private async Task OnSearch()
        {
            CurrentPage = 1;
            await LoadOrdersAsync();
        }

        private async Task OnStatusFilterChanged(OrderStatus? status)
        {
            FilterStatus = status;
            CurrentPage = 1;
            await LoadOrdersAsync();
        }

        private async Task OnPaymentStatusFilterChanged(PaymentStatus? status)
        {
            FilterPaymentStatus = status;
            CurrentPage = 1;
            await LoadOrdersAsync();
        }

        private async Task OnDateFilterChanged()
        {
            CurrentPage = 1;
            await LoadOrdersAsync();
        }

        private async Task OnPageChanged(int page)
        {
            CurrentPage = page;
            await LoadOrdersAsync();
        }

        private async Task OnSortChanged(string field)
        {
            if (field == SortField)
            {
                SortAscending = !SortAscending;
            }
            else
            {
                SortField = field;
                SortAscending = true;
            }

            await LoadOrdersAsync();
        }

        private void NavigateToOrderDetail(Guid id)
        {
            NavigationManager.NavigateTo($"/orders/{id}");
        }

        private async Task UpdateOrderStatus(Guid id, OrderStatus status)
        {
            await OrderAppService.UpdateOrderStatusAsync(id, new UpdateOrderStatusDto { Status = status });
            await LoadOrdersAsync();
        }

        private async Task UpdatePaymentStatus(Guid id, PaymentStatus status)
        {
            await OrderAppService.UpdatePaymentStatusAsync(id, new UpdatePaymentStatusDto 
            { 
                PaymentStatus = status,
                PaymentMethod = "Manual Update"
            });
            await LoadOrdersAsync();
        }

        private async Task MarkAsShipped(Guid id)
        {
            await OrderAppService.MarkAsShippedAsync(id, new ShippingUpdateDto 
            { 
                ShippingMethod = "Standard Shipping"
            });
            await LoadOrdersAsync();
        }

        private async Task MarkAsDelivered(Guid id)
        {
            await OrderAppService.MarkAsDeliveredAsync(id);
            await LoadOrdersAsync();
        }

        private async Task MarkAsCompleted(Guid id)
        {
            await OrderAppService.MarkAsCompletedAsync(id);
            await LoadOrdersAsync();
        }

        private async Task CancelOrder(Guid id)
        {
            var confirmed = await Message.Confirm("Are you sure you want to cancel this order?");
            
            if (confirmed)
            {
                await OrderAppService.CancelOrderAsync(id, new CancelOrderDto { Notes = "Cancelled by admin" });
                await LoadOrdersAsync();
            }
        }

        private string GetOrderStatusLabel(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Pending",
                OrderStatus.Processing => "Processing",
                OrderStatus.Shipped => "Shipped",
                OrderStatus.Delivered => "Delivered",
                OrderStatus.Completed => "Completed",
                OrderStatus.Cancelled => "Cancelled",
                OrderStatus.Refunded => "Refunded",
                _ => "Unknown"
            };
        }

        private string GetOrderStatusClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "bg-warning text-dark",
                OrderStatus.Processing => "bg-info",
                OrderStatus.Shipped => "bg-primary",
                OrderStatus.Delivered => "bg-success",
                OrderStatus.Completed => "bg-success",
                OrderStatus.Cancelled => "bg-danger",
                OrderStatus.Refunded => "bg-secondary",
                _ => "bg-secondary"
            };
        }

        private string GetPaymentStatusLabel(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Pending",
                PaymentStatus.Authorized => "Authorized",
                PaymentStatus.Paid => "Paid",
                PaymentStatus.PartiallyRefunded => "Partially Refunded",
                PaymentStatus.Refunded => "Refunded",
                PaymentStatus.Voided => "Voided",
                _ => "Unknown"
            };
        }

        private string GetPaymentStatusClass(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "bg-warning text-dark",
                PaymentStatus.Authorized => "bg-info",
                PaymentStatus.Paid => "bg-success",
                PaymentStatus.PartiallyRefunded => "bg-warning text-dark",
                PaymentStatus.Refunded => "bg-danger",
                PaymentStatus.Voided => "bg-secondary",
                _ => "bg-secondary"
            };
        }
    }
}
