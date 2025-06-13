using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Categories.Dtos;

public class CategoryDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
} 