using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp.Content;

namespace EcommercePlatform.Categories.Dtos;

public class CreateUpdateCategoryDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    public IRemoteStreamContent? Image { get; set; }

    public bool IsActive { get; set; }

    public Guid? ParentId { get; set; }

    public int DisplayOrder { get; set; }
} 