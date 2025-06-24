//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Volo.Abp.Application.Dtos;
//using Volo.Abp.Application.Services;
//using EcommercePlatform.Enums;

//namespace EcommercePlatform.Application.Contracts.Services
//{
//    /// <summary>
//    /// Interface for the dynamic pricing application service.
//    /// </summary>
//    public interface IDynamicPricingAppService : IApplicationService
//    {
//        /// <summary>
//        /// Gets a dynamic pricing rule by ID.
//        /// </summary>
//        Task<DynamicPriceRuleDto> GetAsync(Guid id);

//        /// <summary>
//        /// Gets a list of all dynamic pricing rules.
//        /// </summary>
//        Task<List<DynamicPriceRuleDto>> GetListAsync();

//        /// <summary>
//        /// Gets a paged list of dynamic pricing rules.
//        /// </summary>
//        Task<PagedResultDto<DynamicPriceRuleDto>> GetPagedListAsync(PagedAndSortedResultRequestDto input);

//        /// <summary>
//        /// Gets a list of active dynamic pricing rules.
//        /// </summary>
//        Task<List<DynamicPriceRuleDto>> GetActiveRulesAsync();

//        /// <summary>
//        /// Creates a new dynamic pricing rule.
//        /// </summary>
//        Task<DynamicPriceRuleDto> CreateAsync(CreateDynamicPriceRuleDto input);

//        /// <summary>
//        /// Updates an existing dynamic pricing rule.
//        /// </summary>
//        Task<DynamicPriceRuleDto> UpdateAsync(Guid id, UpdateDynamicPriceRuleDto input);

//        /// <summary>
//        /// Deletes a dynamic pricing rule.
//        /// </summary>
//        Task DeleteAsync(Guid id);

//        /// <summary>
//        /// Activates a dynamic pricing rule.
//        /// </summary>
//        Task<DynamicPriceRuleDto> ActivateAsync(Guid id);

//        /// <summary>
//        /// Deactivates a dynamic pricing rule.
//        /// </summary>
//        Task<DynamicPriceRuleDto> DeactivateAsync(Guid id);

//        /// <summary>
//        /// Sets the validity period for a dynamic pricing rule.
//        /// </summary>
//        Task<DynamicPriceRuleDto> SetValidityPeriodAsync(Guid id, SetRuleValidityPeriodDto input);

//        /// <summary>
//        /// Updates the priority of a dynamic pricing rule.
//        /// </summary>
//        Task<DynamicPriceRuleDto> UpdatePriorityAsync(Guid id, UpdateRulePriorityDto input);

//        /// <summary>
//        /// Calculates the dynamic price for a product.
//        /// </summary>
//        Task<decimal> CalculateDynamicPriceAsync(Guid productId);
//    }

//    /// <summary>
//    /// DTO for dynamic pricing rule data.
//    /// </summary>
//    public class DynamicPriceRuleDto : EntityDto<Guid>
//    {
//        /// <summary>
//        /// The name of the dynamic pricing rule.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the rule.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The type of the dynamic pricing rule.
//        /// </summary>
//        public DynamicPriceRuleType RuleType { get; set; }

//        /// <summary>
//        /// Parameters for the rule in JSON format (e.g., {"Percentage": 10, "MinQuantity": 5}).
//        /// </summary>
//        public string RuleParameters { get; set; }

//        /// <summary>
//        /// Indicates whether the rule is active.
//        /// </summary>
//        public bool IsActive { get; set; }

//        /// <summary>
//        /// The priority of the rule (lower value means higher priority).
//        /// </summary>
//        public int Priority { get; set; }

//        /// <summary>
//        /// The start date and time for the rule's validity.
//        /// </summary>
//        public DateTime? StartDate { get; set; }

//        /// <summary>
//        /// The end date and time for the rule's validity.
//        /// </summary>
//        public DateTime? EndDate { get; set; }

//        /// <summary>
//        /// The creation time of the dynamic price rule.
//        /// </summary>
//        public DateTime CreationTime { get; set; }
//    }

//    /// <summary>
//    /// DTO for creating a dynamic pricing rule.
//    /// </summary>
//    public class CreateDynamicPriceRuleDto
//    {
//        /// <summary>
//        /// The name of the rule.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the rule.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The type of the dynamic pricing rule.
//        /// </summary>
//        public DynamicPriceRuleType RuleType { get; set; }

//        /// <summary>
//        /// Parameters for the rule in JSON format.
//        /// </summary>
//        public string RuleParameters { get; set; }
//    }

//    /// <summary>
//    /// DTO for updating a dynamic pricing rule.
//    /// </summary>
//    public class UpdateDynamicPriceRuleDto
//    {
//        /// <summary>
//        /// The name of the rule.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the rule.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The type of the dynamic pricing rule.
//        /// </summary>
//        public DynamicPriceRuleType RuleType { get; set; }

//        /// <summary>
//        /// Parameters for the rule in JSON format.
//        /// </summary>
//        public string RuleParameters { get; set; }
//    }

//    /// <summary>
//    /// DTO for setting the validity period of a dynamic pricing rule.
//    /// </summary>
//    public class SetRuleValidityPeriodDto
//    {
//        /// <summary>
//        /// The start date and time for the rule's validity.
//        /// </summary>
//        public DateTime? StartDate { get; set; }

//        /// <summary>
//        /// The end date and time for the rule's validity.
//        /// </summary>
//        public DateTime? EndDate { get; set; }
//    }

//    /// <summary>
//    /// DTO for updating the priority of a dynamic pricing rule.
//    /// </summary>
//    public class UpdateRulePriorityDto
//    {
//        /// <summary>
//        /// The new priority of the rule.
//        /// </summary>
//        public int Priority { get; set; }
//    }
//} 