using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Categories.Dtos;

public class CategoryLookupDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string DisplayName => Name;
}