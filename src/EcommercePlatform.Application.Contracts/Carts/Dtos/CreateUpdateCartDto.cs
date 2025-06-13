using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Carts.Dtos;

public class CreateUpdateCartDto
{
    [Required]
    public Guid UserId { get; set; }
} 