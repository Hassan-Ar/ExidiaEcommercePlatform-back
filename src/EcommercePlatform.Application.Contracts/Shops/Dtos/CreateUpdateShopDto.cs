using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Shops.Dtos;

public class CreateUpdateShopDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [Required]
    [StringLength(256)]
    public string SellerId { get; set; }

    public bool IsActive { get; set; }
} 