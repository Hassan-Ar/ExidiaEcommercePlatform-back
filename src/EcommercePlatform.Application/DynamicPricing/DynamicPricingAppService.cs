using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EcommercePlatform.DynamicPricing.Dtos;

namespace EcommercePlatform.DynamicPricing;

public class DynamicPricingAppService :
    CrudAppService<
        DynamicPriceRule,
        DynamicPriceRuleDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateDynamicPriceRuleDto>,
    IDynamicPricingAppService
{
    public DynamicPricingAppService(IRepository<DynamicPriceRule, Guid> repository) : base(repository)
    {
    }
} 