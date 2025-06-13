using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Application.Services;
using EcommercePlatform.Enums;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components;

namespace EcommercePlatform.Blazor.Pages.Products
{
    public partial class ProductList : AbpComponentBase
    {
        [Inject]
        protected IProductAppService ProductAppService { get; set; }

        [Inject]
        protected ICategoryAppService CategoryAppService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private List<ProductDto> Products { get; set; } = new List<ProductDto>();
        private List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        private int TotalCount { get; set; }
        private int CurrentPage { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private string SearchTerm { get; set; }
        private Guid? SelectedCategoryId { get; set; }
        private string SortField { get; set; } = "Name";
        private bool SortAscending { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var result = await CategoryAppService.GetListAsync();
            Categories = result;
        }

        private async Task LoadProductsAsync()
        {
            var skipCount = (CurrentPage - 1) * PageSize;
            var sorting = SortAscending ? SortField : SortField + " DESC";

            var input = new PagedAndSortedResultRequestDto
            {
                SkipCount = skipCount,
                MaxResultCount = PageSize,
                Sorting = sorting
            };

            PagedResultDto<ProductDto> result;

            if (SelectedCategoryId.HasValue)
            {
                var categoryProducts = await ProductAppService.GetByCategoryAsync(SelectedCategoryId.Value);
                
                // Manual paging for filtered results
                var filteredProducts = string.IsNullOrWhiteSpace(SearchTerm) 
                    ? categoryProducts 
                    : categoryProducts.FindAll(p => p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                                                  p.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                
                TotalCount = filteredProducts.Count;
                Products = filteredProducts.Skip(skipCount).Take(PageSize).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                // In a real implementation, this would be handled by a backend search endpoint
                // For now, we'll get all products and filter client-side
                var allProducts = await ProductAppService.GetListAsync();
                var filteredProducts = allProducts.FindAll(p => p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                                                              p.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                
                TotalCount = filteredProducts.Count;
                Products = filteredProducts.Skip(skipCount).Take(PageSize).ToList();
            }
            else
            {
                result = await ProductAppService.GetPagedListAsync(input);
                Products = result.Items;
                TotalCount = result.TotalCount;
            }
        }

        private async Task OnSearch()
        {
            CurrentPage = 1;
            await LoadProductsAsync();
        }

        private async Task OnCategoryChanged(Guid? categoryId)
        {
            SelectedCategoryId = categoryId;
            CurrentPage = 1;
            await LoadProductsAsync();
        }

        private async Task OnPageChanged(int page)
        {
            CurrentPage = page;
            await LoadProductsAsync();
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

            await LoadProductsAsync();
        }

        private void NavigateToProductDetail(Guid id)
        {
            NavigationManager.NavigateTo($"/products/{id}");
        }

        private void NavigateToCreateProduct()
        {
            NavigationManager.NavigateTo("/products/new");
        }

        private async Task PublishProduct(Guid id)
        {
            await ProductAppService.PublishAsync(id);
            await LoadProductsAsync();
        }

        private async Task UnpublishProduct(Guid id)
        {
            await ProductAppService.UnpublishAsync(id);
            await LoadProductsAsync();
        }

        private async Task DeleteProduct(Guid id)
        {
            var confirmed = await Message.Confirm("Are you sure you want to delete this product?");
            
            if (confirmed)
            {
                await ProductAppService.DeleteAsync(id);
                await LoadProductsAsync();
            }
        }

        private string GetProductTypeLabel(ProductType type)
        {
            return type switch
            {
                ProductType.Physical => "Physical",
                ProductType.Digital => "Digital",
                ProductType.Service => "Service",
                ProductType.Subscription => "Subscription",
                _ => "Unknown"
            };
        }

        private string GetPriceDisplay(decimal basePrice, decimal dynamicPrice)
        {
            if (dynamicPrice != basePrice)
            {
                return $"<span class=\"text-decoration-line-through\">${basePrice:F2}</span> <span class=\"text-danger\">${dynamicPrice:F2}</span>";
            }
            
            return $"${basePrice:F2}";
        }
    }
}
