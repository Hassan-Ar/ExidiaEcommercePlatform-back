using System;

namespace EcommercePlatform.Enums
{
    /// <summary>
    /// Represents the type of dynamic pricing rule in the system.
    /// </summary>
    public enum DynamicPriceRuleType
    {
        /// <summary>
        /// Adjusts price based on time of day or day of week.
        /// </summary>
        TimeBasedPricing = 0,
        
        /// <summary>
        /// Adjusts price based on current inventory levels.
        /// </summary>
        InventoryBasedPricing = 1,
        
        /// <summary>
        /// Adjusts price based on customer behavior and demand.
        /// </summary>
        DemandBasedPricing = 2,
        
        /// <summary>
        /// Adjusts price based on competitor pricing.
        /// </summary>
        CompetitorBasedPricing = 3,
        
        /// <summary>
        /// Adjusts price based on customer loyalty or segments.
        /// </summary>
        CustomerSegmentPricing = 4,
        
        /// <summary>
        /// Adjusts price based on AI-generated recommendations.
        /// </summary>
        AiRecommendedPricing = 5
    }
}
