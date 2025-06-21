using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp.Content;

namespace EcommercePlatform.Products.Dtos;

public class CreateUpdateProductDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    [StringLength(64)]
    public string SKU { get; set; }

    public IRemoteStreamContent? Image { get; set; }

    public bool IsActive { get; set; }

    public Guid CategoryId { get; set; }

    /// <summary>
    /// Optional discount percentage to apply to product price.
    /// </summary>
    [Range(0, 100)]
    public decimal? DiscountPercent { get; set; }
} 