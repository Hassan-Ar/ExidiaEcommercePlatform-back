using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.DynamicPricing.Dtos;

public class DynamicPriceRuleDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string RuleType { get; set; }
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
} 