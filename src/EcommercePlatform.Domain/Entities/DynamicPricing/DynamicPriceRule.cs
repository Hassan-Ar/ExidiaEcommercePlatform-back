using System;
using EcommercePlatform.Products;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.DynamicPricing;

public class DynamicPriceRule : FullAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public string RuleType { get; set; }
    public string Condition { get; set; }
    public decimal AdjustmentValue { get; set; }
    public string AdjustmentType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }

    protected DynamicPriceRule()
    {
    }

    public DynamicPriceRule(
        Guid id,
        Guid productId,
        string ruleType,
        string condition,
        decimal adjustmentValue,
        string adjustmentType,
        DateTime? startDate,
        DateTime? endDate,
        int priority,
        string description,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        ProductId = productId;
        RuleType = ruleType;
        Condition = condition;
        AdjustmentValue = adjustmentValue;
        AdjustmentType = adjustmentType;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = true;
        Priority = priority;
        Description = description;
    }

    public bool IsValid()
    {
        if (!IsActive)
        {
            return false;
        }

        var now = DateTime.UtcNow;
        if (StartDate.HasValue && now < StartDate.Value)
        {
            return false;
        }

        if (EndDate.HasValue && now > EndDate.Value)
        {
            return false;
        }

        return true;
    }

    public decimal CalculateAdjustedPrice(decimal basePrice)
    {
        if (!IsValid())
        {
            return basePrice;
        }

        return AdjustmentType switch
        {
            "Percentage" => basePrice * (1 + AdjustmentValue / 100),
            "Fixed" => basePrice + AdjustmentValue,
            "Multiplier" => basePrice * AdjustmentValue,
            _ => basePrice
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateTimeRange(DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
        {
            throw new ArgumentException("Start date cannot be after end date");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    public void UpdateAdjustment(decimal adjustmentValue, string adjustmentType)
    {
        if (adjustmentType == "Percentage" && adjustmentValue < -100)
        {
            throw new ArgumentException("Percentage adjustment cannot be less than -100%");
        }

        AdjustmentValue = adjustmentValue;
        AdjustmentType = adjustmentType;
    }
} 