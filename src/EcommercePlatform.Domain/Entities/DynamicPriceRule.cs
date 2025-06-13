using System;
using System.Collections.Generic;
using EcommercePlatform.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents a dynamic pricing rule in the e-commerce platform.
    /// </summary>
    public class DynamicPriceRule : FullAuditedEntity<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this rule belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The name of the rule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of dynamic pricing rule.
        /// </summary>
        public DynamicPriceRuleType RuleType { get; set; }

        /// <summary>
        /// The parameters for the rule (in JSON format).
        /// </summary>
        public string RuleParameters { get; set; }

        /// <summary>
        /// Indicates whether the rule is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The start date for the rule's validity period.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date for the rule's validity period.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The priority of the rule (lower numbers have higher priority).
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Creates a new DynamicPriceRule instance.
        /// </summary>
        protected DynamicPriceRule()
        {
        }

        /// <summary>
        /// Creates a new DynamicPriceRule instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the rule.</param>
        /// <param name="tenantId">The tenant ID that this rule belongs to.</param>
        /// <param name="name">The name of the rule.</param>
        /// <param name="ruleType">The type of dynamic pricing rule.</param>
        /// <param name="ruleParameters">The parameters for the rule.</param>
        /// <param name="description">The description of the rule.</param>
        public DynamicPriceRule(
            Guid id,
            Guid? tenantId,
            string name,
            DynamicPriceRuleType ruleType,
            string ruleParameters,
            string description = null)
            : base(id)
        {
            TenantId = tenantId;
            Name = name;
            Description = description;
            RuleType = ruleType;
            RuleParameters = ruleParameters;
            IsActive = true;
            Priority = 100; // Default priority
        }

        /// <summary>
        /// Activates the rule.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the rule.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Sets the validity period for the rule.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public void SetValidityPeriod(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Updates the rule's priority.
        /// </summary>
        /// <param name="priority">The new priority.</param>
        public void UpdatePriority(int priority)
        {
            if (priority < 0)
            {
                throw new ArgumentException("Priority cannot be negative.", nameof(priority));
            }

            Priority = priority;
        }

        /// <summary>
        /// Updates the rule's parameters.
        /// </summary>
        /// <param name="ruleParameters">The new parameters.</param>
        public void UpdateParameters(string ruleParameters)
        {
            if (string.IsNullOrEmpty(ruleParameters))
            {
                throw new ArgumentException("Rule parameters cannot be null or empty.", nameof(ruleParameters));
            }

            RuleParameters = ruleParameters;
        }

        /// <summary>
        /// Determines whether the rule is valid at the specified date.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the rule is valid at the specified date, false otherwise.</returns>
        public bool IsValidAt(DateTime date)
        {
            if (!IsActive)
            {
                return false;
            }

            if (StartDate.HasValue && date < StartDate.Value)
            {
                return false;
            }

            if (EndDate.HasValue && date > EndDate.Value)
            {
                return false;
            }

            return true;
        }
    }
}
