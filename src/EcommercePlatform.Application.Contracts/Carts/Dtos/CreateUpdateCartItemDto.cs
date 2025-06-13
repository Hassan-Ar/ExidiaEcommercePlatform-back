using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Carts.Dtos;

public class CreateUpdateCartItemDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
} 