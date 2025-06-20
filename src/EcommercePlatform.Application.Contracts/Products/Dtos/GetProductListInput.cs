using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Products.Dtos;

public class GetProductListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? CategoryId { get; set; }
    public bool? IsActive { get; set; }
} 