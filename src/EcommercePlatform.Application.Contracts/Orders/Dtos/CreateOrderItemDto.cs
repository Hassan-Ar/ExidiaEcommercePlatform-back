using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Orders.Dtos;

public class CreateOrderItemDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
} 