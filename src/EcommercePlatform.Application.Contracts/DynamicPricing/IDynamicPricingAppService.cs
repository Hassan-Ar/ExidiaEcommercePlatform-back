using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using EcommercePlatform.DynamicPricing.Dtos;

namespace EcommercePlatform.DynamicPricing;

public interface IDynamicPricingAppService :
    ICrudAppService<
        DynamicPriceRuleDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateDynamicPriceRuleDto>
{
    
} 