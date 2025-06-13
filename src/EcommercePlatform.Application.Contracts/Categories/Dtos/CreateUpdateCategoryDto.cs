using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Categories.Dtos;

public class CreateUpdateCategoryDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [StringLength(512)]
    public string ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public Guid? ParentCategoryId { get; set; }
} 