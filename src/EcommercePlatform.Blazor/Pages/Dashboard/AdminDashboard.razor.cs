using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Application.Services;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace EcommercePlatform.Blazor.Pages.Dashboard
{
    public partial class AdminDashboard : AbpComponentBase
    {
        [Inject]
        protected IProductAppService ProductAppService { get; set; }

        [Inject]
        protected IOrderAppService OrderAppService { get; set; }

        [Inject]
        protected IShopAppService ShopAppService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private DashboardStatsDto Stats { get; set; } = new DashboardStatsDto();
        private List<RecentOrderDto> RecentOrders { get; set; } = new List<RecentOrderDto>();
        private List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
        private List<SalesChartDataDto> SalesData { get; set; } = new List<SalesChartDataDto>();
        private bool IsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            
            try
            {
                await LoadDashboardDataAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadDashboardDataAsync()
        {
            // In a real implementation, these would be actual API calls
            // For now, we'll simulate the data
            
            // Simulate loading dashboard stats
            await Task.Delay(500);
            Stats = new DashboardStatsDto
            {
                TotalSales = 24850.75m,
                OrdersCount = 156,
                ProductsCount = 89,
                CustomersCount = 243,
                AverageOrderValue = 159.30m,
                ConversionRate = 3.2m
            };
            
            // Simulate loading recent orders
            await Task.Delay(300);
            RecentOrders = new List<RecentOrderDto>
            {
                new RecentOrderDto { Id = Guid.NewGuid(), OrderNumber = "ORD-2025-0001", CustomerName = "John Smith", TotalAmount = 245.99m, Status = Enums.OrderStatus.Delivered, CreationTime = DateTime.Now.AddDays(-1) },
                new RecentOrderDto { Id = Guid.NewGuid(), OrderNumber = "ORD-2025-0002", CustomerName = "Alice Johnson", TotalAmount = 189.50m, Status = Enums.OrderStatus.Processing, CreationTime = DateTime.Now.AddDays(-1).AddHours(2) },
                new RecentOrderDto { Id = Guid.NewGuid(), OrderNumber = "ORD-2025-0003", CustomerName = "Robert Brown", TotalAmount = 349.99m, Status = Enums.OrderStatus.Shipped, CreationTime = DateTime.Now.AddDays(-2) },
                new RecentOrderDto { Id = Guid.NewGuid(), OrderNumber = "ORD-2025-0004", CustomerName = "Emily Davis", TotalAmount = 129.95m, Status = Enums.OrderStatus.Pending, CreationTime = DateTime.Now.AddHours(-5) },
                new RecentOrderDto { Id = Guid.NewGuid(), OrderNumber = "ORD-2025-0005", CustomerName = "Michael Wilson", TotalAmount = 459.85m, Status = Enums.OrderStatus.Completed, CreationTime = DateTime.Now.AddDays(-3) }
            };
            
            // Simulate loading top products
            await Task.Delay(300);
            TopProducts = new List<TopProductDto>
            {
                new TopProductDto { Id = Guid.NewGuid(), Name = "Premium Wireless Headphones", TotalSales = 4250.00m, UnitsSold = 25, AverageRating = 4.8m },
                new TopProductDto { Id = Guid.NewGuid(), Name = "Smart Home Hub", TotalSales = 3899.95m, UnitsSold = 19, AverageRating = 4.5m },
                new TopProductDto { Id = Guid.NewGuid(), Name = "Ultra HD Monitor", TotalSales = 3599.97m, UnitsSold = 9, AverageRating = 4.9m },
                new TopProductDto { Id = Guid.NewGuid(), Name = "Ergonomic Office Chair", TotalSales = 2999.80m, UnitsSold = 12, AverageRating = 4.7m },
                new TopProductDto { Id = Guid.NewGuid(), Name = "Fitness Tracker Pro", TotalSales = 1999.75m, UnitsSold = 15, AverageRating = 4.6m }
            };
            
            // Simulate loading sales chart data
            await Task.Delay(300);
            SalesData = new List<SalesChartDataDto>
            {
                new SalesChartDataDto { Date = DateTime.Now.AddDays(-6).ToString("MMM dd"), Sales = 3250.50m },
                new SalesChartDataDto { Date = DateTime.Now.AddDays(-5).ToString("MMM dd"), Sales = 4120.75m },
                new SalesChartDataDto { Date = DateTime.Now.AddDays(-4).ToString("MMM dd"), Sales = 3890.25m },
                new SalesChartDataDto { Date = DateTime.Now.AddDays(-3).ToString("MMM dd"), Sales = 4350.00m },
                new SalesChartDataDto { Date = DateTime.Now.AddDays(-2).ToString("MMM dd"), Sales = 3980.50m },
                new SalesChartDataDto { Date = DateTime.Now.AddDays(-1).ToString("MMM dd"), Sales = 4560.25m },
                new SalesChartDataDto { Date = DateTime.Now.ToString("MMM dd"), Sales = 4699.50m }
            };
        }

        private void NavigateToOrders()
        {
            NavigationManager.NavigateTo("/orders");
        }

        private void NavigateToProducts()
        {
            NavigationManager.NavigateTo("/products");
        }

        private void NavigateToCustomers()
        {
            NavigationManager.NavigateTo("/customers");
        }

        private void NavigateToOrderDetail(Guid id)
        {
            NavigationManager.NavigateTo($"/orders/{id}");
        }

        private void NavigateToProductDetail(Guid id)
        {
            NavigationManager.NavigateTo($"/products/{id}");
        }

        private string GetOrderStatusClass(Enums.OrderStatus status)
        {
            return status switch
            {
                Enums.OrderStatus.Pending => "bg-warning text-dark",
                Enums.OrderStatus.Processing => "bg-info",
                Enums.OrderStatus.Shipped => "bg-primary",
                Enums.OrderStatus.Delivered => "bg-success",
                Enums.OrderStatus.Completed => "bg-success",
                Enums.OrderStatus.Cancelled => "bg-danger",
                Enums.OrderStatus.Refunded => "bg-secondary",
                _ => "bg-secondary"
            };
        }

        private string GetOrderStatusLabel(Enums.OrderStatus status)
        {
            return status switch
            {
                Enums.OrderStatus.Pending => "Pending",
                Enums.OrderStatus.Processing => "Processing",
                Enums.OrderStatus.Shipped => "Shipped",
                Enums.OrderStatus.Delivered => "Delivered",
                Enums.OrderStatus.Completed => "Completed",
                Enums.OrderStatus.Cancelled => "Cancelled",
                Enums.OrderStatus.Refunded => "Refunded",
                _ => "Unknown"
            };
        }
    }

    public class DashboardStatsDto
    {
        public decimal TotalSales { get; set; }
        public int OrdersCount { get; set; }
        public int ProductsCount { get; set; }
        public int CustomersCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal ConversionRate { get; set; }
    }

    public class RecentOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public Enums.OrderStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class TopProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalSales { get; set; }
        public int UnitsSold { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class SalesChartDataDto
    {
        public string Date { get; set; }
        public decimal Sales { get; set; }
    }
}
