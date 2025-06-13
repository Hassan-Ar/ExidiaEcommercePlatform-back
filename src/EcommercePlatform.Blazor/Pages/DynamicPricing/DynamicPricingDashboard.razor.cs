using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Application.Services;
using EcommercePlatform.Enums;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace EcommercePlatform.Blazor.Pages.DynamicPricing
{
    public partial class DynamicPricingDashboard : AbpComponentBase
    {
        [Inject]
        protected IDynamicPricingAppService DynamicPricingAppService { get; set; }

        [Inject]
        protected IProductAppService ProductAppService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private List<DynamicPriceRuleDto> Rules { get; set; } = new List<DynamicPriceRuleDto>();
        private int TotalCount { get; set; }
        private int CurrentPage { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private string SearchTerm { get; set; }
        private string SortField { get; set; } = "Priority";
        private bool SortAscending { get; set; } = true;
        private bool ShowOnlyActive { get; set; } = true;
        private CreateDynamicPriceRuleDto NewRule { get; set; } = new CreateDynamicPriceRuleDto();
        private bool IsCreatingRule { get; set; }
        private bool IsEditingRule { get; set; }
        private Guid? EditingRuleId { get; set; }
        private UpdateDynamicPriceRuleDto EditRule { get; set; } = new UpdateDynamicPriceRuleDto();
        private SetRuleValidityPeriodDto ValidityPeriod { get; set; } = new SetRuleValidityPeriodDto();
        private UpdateRulePriorityDto PriorityUpdate { get; set; } = new UpdateRulePriorityDto();

        protected override async Task OnInitializedAsync()
        {
            await LoadRulesAsync();
        }

        private async Task LoadRulesAsync()
        {
            var skipCount = (CurrentPage - 1) * PageSize;
            var sorting = SortAscending ? SortField : SortField + " DESC";

            var input = new Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto
            {
                SkipCount = skipCount,
                MaxResultCount = PageSize,
                Sorting = sorting
            };

            var result = await DynamicPricingAppService.GetPagedListAsync(input);
            Rules = result.Items;
            TotalCount = result.TotalCount;

            // Apply client-side filtering if needed
            if (ShowOnlyActive || !string.IsNullOrWhiteSpace(SearchTerm))
            {
                var filteredRules = new List<DynamicPriceRuleDto>();
                
                foreach (var rule in Rules)
                {
                    bool includeRule = true;
                    
                    if (ShowOnlyActive && !rule.IsActive)
                    {
                        includeRule = false;
                    }
                    
                    if (!string.IsNullOrWhiteSpace(SearchTerm) && 
                        !rule.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) &&
                        !rule.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        includeRule = false;
                    }
                    
                    if (includeRule)
                    {
                        filteredRules.Add(rule);
                    }
                }
                
                Rules = filteredRules;
                TotalCount = filteredRules.Count;
            }
        }

        private async Task OnSearch()
        {
            CurrentPage = 1;
            await LoadRulesAsync();
        }

        private async Task OnShowActiveToggle()
        {
            ShowOnlyActive = !ShowOnlyActive;
            CurrentPage = 1;
            await LoadRulesAsync();
        }

        private async Task OnPageChanged(int page)
        {
            CurrentPage = page;
            await LoadRulesAsync();
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

            await LoadRulesAsync();
        }

        private void ShowCreateRuleForm()
        {
            IsCreatingRule = true;
            NewRule = new CreateDynamicPriceRuleDto();
        }

        private void CancelCreateRule()
        {
            IsCreatingRule = false;
        }

        private async Task CreateRuleAsync()
        {
            try
            {
                await DynamicPricingAppService.CreateAsync(NewRule);
                IsCreatingRule = false;
                await LoadRulesAsync();
                await Message.Success("Dynamic pricing rule created successfully!");
            }
            catch (Exception ex)
            {
                await Message.Error("Failed to create rule: " + ex.Message);
            }
        }

        private void ShowEditRuleForm(DynamicPriceRuleDto rule)
        {
            IsEditingRule = true;
            EditingRuleId = rule.Id;
            EditRule = new UpdateDynamicPriceRuleDto
            {
                Name = rule.Name,
                Description = rule.Description,
                RuleType = rule.RuleType,
                RuleParameters = rule.RuleParameters
            };
            ValidityPeriod = new SetRuleValidityPeriodDto
            {
                StartDate = rule.StartDate,
                EndDate = rule.EndDate
            };
            PriorityUpdate = new UpdateRulePriorityDto
            {
                Priority = rule.Priority
            };
        }

        private void CancelEditRule()
        {
            IsEditingRule = false;
            EditingRuleId = null;
        }

        private async Task UpdateRuleAsync()
        {
            if (!EditingRuleId.HasValue)
            {
                return;
            }

            try
            {
                await DynamicPricingAppService.UpdateAsync(EditingRuleId.Value, EditRule);
                await DynamicPricingAppService.SetValidityPeriodAsync(EditingRuleId.Value, ValidityPeriod);
                await DynamicPricingAppService.UpdatePriorityAsync(EditingRuleId.Value, PriorityUpdate);
                
                IsEditingRule = false;
                EditingRuleId = null;
                await LoadRulesAsync();
                await Message.Success("Dynamic pricing rule updated successfully!");
            }
            catch (Exception ex)
            {
                await Message.Error("Failed to update rule: " + ex.Message);
            }
        }

        private async Task ActivateRuleAsync(Guid id)
        {
            try
            {
                await DynamicPricingAppService.ActivateAsync(id);
                await LoadRulesAsync();
            }
            catch (Exception ex)
            {
                await Message.Error("Failed to activate rule: " + ex.Message);
            }
        }

        private async Task DeactivateRuleAsync(Guid id)
        {
            try
            {
                await DynamicPricingAppService.DeactivateAsync(id);
                await LoadRulesAsync();
            }
            catch (Exception ex)
            {
                await Message.Error("Failed to deactivate rule: " + ex.Message);
            }
        }

        private async Task DeleteRuleAsync(Guid id)
        {
            var confirmed = await Message.Confirm("Are you sure you want to delete this pricing rule?");
            
            if (confirmed)
            {
                try
                {
                    await DynamicPricingAppService.DeleteAsync(id);
                    await LoadRulesAsync();
                    await Message.Success("Dynamic pricing rule deleted successfully!");
                }
                catch (Exception ex)
                {
                    await Message.Error("Failed to delete rule: " + ex.Message);
                }
            }
        }

        private string GetRuleTypeLabel(DynamicPriceRuleType type)
        {
            return type switch
            {
                DynamicPriceRuleType.TimeBasedDiscount => "Time-based Discount",
                DynamicPriceRuleType.InventoryBasedPricing => "Inventory-based Pricing",
                DynamicPriceRuleType.CompetitorBasedPricing => "Competitor-based Pricing",
                DynamicPriceRuleType.DemandBasedPricing => "Demand-based Pricing",
                DynamicPriceRuleType.CustomerSegmentPricing => "Customer Segment Pricing",
                DynamicPriceRuleType.BundlePricing => "Bundle Pricing",
                DynamicPriceRuleType.AiRecommendedPricing => "AI-recommended Pricing",
                _ => "Unknown"
            };
        }

        private string GetValidityPeriodDisplay(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue && !endDate.HasValue)
            {
                return "Always active";
            }
            
            if (startDate.HasValue && !endDate.HasValue)
            {
                return $"From {startDate.Value.ToShortDateString()} onwards";
            }
            
            if (!startDate.HasValue && endDate.HasValue)
            {
                return $"Until {endDate.Value.ToShortDateString()}";
            }
            
            return $"{startDate.Value.ToShortDateString()} - {endDate.Value.ToShortDateString()}";
        }
    }
}
