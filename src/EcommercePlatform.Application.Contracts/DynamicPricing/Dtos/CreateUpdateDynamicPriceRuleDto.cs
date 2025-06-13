using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.DynamicPricing.Dtos;

public class CreateUpdateDynamicPriceRuleDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    [StringLength(64)]
    public string RuleType { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }
} 