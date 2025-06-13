using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Entities;
using EcommercePlatform.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// Domain service for managing dynamic pricing rules.
    /// </summary>
    public class DynamicPricingManager : DomainService, IDynamicPricingManager
    {
        private readonly IRepository<DynamicPriceRule, Guid> _dynamicPriceRuleRepository;
        private readonly IRepository<Product, Guid> _productRepository;

        /// <summary>
        /// Creates a new instance of DynamicPricingManager.
        /// </summary>
        /// <param name="dynamicPriceRuleRepository">The dynamic price rule repository.</param>
        /// <param name="productRepository">The product repository.</param>
        public DynamicPricingManager(
            IRepository<DynamicPriceRule, Guid> dynamicPriceRuleRepository,
            IRepository<Product, Guid> productRepository)
        {
            _dynamicPriceRuleRepository = dynamicPriceRuleRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Creates a new dynamic pricing rule.
        /// </summary>
        /// <param name="name">The name of the rule.</param>
        /// <param name="ruleType">The type of dynamic pricing rule.</param>
        /// <param name="ruleParameters">The parameters for the rule.</param>
        /// <param name="description">The description of the rule.</param>
        /// <returns>The newly created dynamic pricing rule.</returns>
        public async Task<DynamicPriceRule> CreateRuleAsync(
            string name,
            DynamicPriceRuleType ruleType,
            string ruleParameters,
            string description = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Rule name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(ruleParameters))
            {
                throw new ArgumentException("Rule parameters cannot be empty.", nameof(ruleParameters));
            }

            // Create rule
            var rule = new DynamicPriceRule(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                name,
                ruleType,
                ruleParameters,
                description);

            return await _dynamicPriceRuleRepository.InsertAsync(rule);
        }

        /// <summary>
        /// Updates an existing dynamic pricing rule.
        /// </summary>
        /// <param name="id">The ID of the rule to update.</param>
        /// <param name="name">The new name of the rule.</param>
        /// <param name="ruleType">The new type of dynamic pricing rule.</param>
        /// <param name="ruleParameters">The new parameters for the rule.</param>
        /// <param name="description">The new description of the rule.</param>
        /// <returns>The updated dynamic pricing rule.</returns>
        public async Task<DynamicPriceRule> UpdateRuleAsync(
            Guid id,
            string name,
            DynamicPriceRuleType ruleType,
            string ruleParameters,
            string description = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Rule name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(ruleParameters))
            {
                throw new ArgumentException("Rule parameters cannot be empty.", nameof(ruleParameters));
            }

            // Get rule
            var rule = await _dynamicPriceRuleRepository.GetAsync(id);

            // Update rule properties
            rule.Name = name;
            rule.Description = description;
            rule.RuleType = ruleType;
            rule.UpdateParameters(ruleParameters);

            return await _dynamicPriceRuleRepository.UpdateAsync(rule);
        }

        /// <summary>
        /// Activates a dynamic pricing rule.
        /// </summary>
        /// <param name="id">The ID of the rule to activate.</param>
        /// <returns>The activated dynamic pricing rule.</returns>
        public async Task<DynamicPriceRule> ActivateRuleAsync(Guid id)
        {
            var rule = await _dynamicPriceRuleRepository.GetAsync(id);
            rule.Activate();
            return await _dynamicPriceRuleRepository.UpdateAsync(rule);
        }

        /// <summary>
        /// Deactivates a dynamic pricing rule.
        /// </summary>
        /// <param name="id">The ID of the rule to deactivate.</param>
        /// <returns>The deactivated dynamic pricing rule.</returns>
        public async Task<DynamicPriceRule> DeactivateRuleAsync(Guid id)
        {
            var rule = await _dynamicPriceRuleRepository.GetAsync(id);
            rule.Deactivate();
            return await _dynamicPriceRuleRepository.UpdateAsync(rule);
        }

        /// <summary>
        /// Sets the validity period for a dynamic pricing rule.
        /// </summary>
        /// <param name="id">The ID of the rule.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The updated dynamic pricing rule.</returns>
        public async Task<DynamicPriceRule> SetRuleValidityPeriodAsync(
            Guid id,
            DateTime? startDate,
            DateTime? endDate)
        {
            var rule = await _dynamicPriceRuleRepository.GetAsync(id);
            rule.SetValidityPeriod(startDate, endDate);
            return await _dynamicPriceRuleRepository.UpdateAsync(rule);
        }

        /// <summary>
        /// Updates the priority of a dynamic pricing rule.
        /// </summary>
        /// <param name="id">The ID of the rule.</param>
        /// <param name="priority">The new priority.</param>
        /// <returns>The updated dynamic pricing rule.</returns>
        public async Task<DynamicPriceRule> UpdateRulePriorityAsync(Guid id, int priority)
        {
            var rule = await _dynamicPriceRuleRepository.GetAsync(id);
            rule.UpdatePriority(priority);
            return await _dynamicPriceRuleRepository.UpdateAsync(rule);
        }

        /// <summary>
        /// Calculates the dynamic price for a product based on active rules.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>The calculated dynamic price.</returns>
        public async Task<decimal> CalculateDynamicPriceAsync(Guid productId)
        {
            // Get product
            var product = await _productRepository.GetAsync(productId);
            
            // Get all active rules
            var now = DateTime.UtcNow;
            var activeRules = await _dynamicPriceRuleRepository.GetListAsync(
                r => r.IsActive &&
                     (!r.StartDate.HasValue || r.StartDate.Value <= now) &&
                     (!r.EndDate.HasValue || r.EndDate.Value >= now));
                //r => r.Priority);

            // If no active rules, return base price
            if (activeRules.Count == 0)
            {
                return product.BasePrice;
            }

            // Apply rules to calculate dynamic price
            decimal dynamicPrice = product.BasePrice;
            
            foreach (var rule in activeRules)
            {
                dynamicPrice = ApplyPricingRule(rule, product, dynamicPrice);
            }

            // Ensure price is not negative
            return Math.Max(0, dynamicPrice);
        }

        /// <summary>
        /// Applies a pricing rule to calculate a new price.
        /// </summary>
        /// <param name="rule">The rule to apply.</param>
        /// <param name="product">The product.</param>
        /// <param name="currentPrice">The current price.</param>
        /// <returns>The new price after applying the rule.</returns>
        private decimal ApplyPricingRule(DynamicPriceRule rule, Product product, decimal currentPrice)
        {
            // This is a simplified implementation
            // In a real application, this would parse the rule parameters and apply complex logic
            
            switch (rule.RuleType)
            {
                case DynamicPriceRuleType.TimeBasedPricing:
                    // Example: 10% discount during off-peak hours
                    return currentPrice * 0.9m;
                
                case DynamicPriceRuleType.InventoryBasedPricing:
                    // Example: 5% increase when stock is low
                    if (product.StockQuantity < 10)
                    {
                        return currentPrice * 1.05m;
                    }
                    break;
                
                case DynamicPriceRuleType.DemandBasedPricing:
                    // Example: 15% increase during high demand
                    return currentPrice * 1.15m;
                
                case DynamicPriceRuleType.CompetitorBasedPricing:
                    // Example: Match competitor price (would require external data)
                    break;
                
                case DynamicPriceRuleType.CustomerSegmentPricing:
                    // Example: 5% discount for loyal customers
                    return currentPrice * 0.95m;
                
                case DynamicPriceRuleType.AiRecommendedPricing:
                    // Example: AI recommended price (would require AI model)
                    break;
            }
            
            return currentPrice;
        }
    }

    /// <summary>
    /// Interface for the dynamic pricing manager domain service.
    /// </summary>
    public interface IDynamicPricingManager : IDomainService
    {
        /// <summary>
        /// Creates a new dynamic pricing rule.
        /// </summary>
        Task<DynamicPriceRule> CreateRuleAsync(
            string name,
            DynamicPriceRuleType ruleType,
            string ruleParameters,
            string description = null);

        /// <summary>
        /// Updates an existing dynamic pricing rule.
        /// </summary>
        Task<DynamicPriceRule> UpdateRuleAsync(
            Guid id,
            string name,
            DynamicPriceRuleType ruleType,
            string ruleParameters,
            string description = null);

        /// <summary>
        /// Activates a dynamic pricing rule.
        /// </summary>
        Task<DynamicPriceRule> ActivateRuleAsync(Guid id);

        /// <summary>
        /// Deactivates a dynamic pricing rule.
        /// </summary>
        Task<DynamicPriceRule> DeactivateRuleAsync(Guid id);

        /// <summary>
        /// Sets the validity period for a dynamic pricing rule.
        /// </summary>
        Task<DynamicPriceRule> SetRuleValidityPeriodAsync(
            Guid id,
            DateTime? startDate,
            DateTime? endDate);

        /// <summary>
        /// Updates the priority of a dynamic pricing rule.
        /// </summary>
        Task<DynamicPriceRule> UpdateRulePriorityAsync(Guid id, int priority);

        /// <summary>
        /// Calculates the dynamic price for a product based on active rules.
        /// </summary>
        Task<decimal> CalculateDynamicPriceAsync(Guid productId);
    }
}
