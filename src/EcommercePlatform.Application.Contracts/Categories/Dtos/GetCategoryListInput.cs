using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Categories.Dtos;

public class GetCategoryListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public bool? IsActive { get; set; }
} 