using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Orders.Dtos;

public class CreateUpdateOrderDto
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [StringLength(256)]
    public string CustomerName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string CustomerEmail { get; set; }

    [Required]
    [StringLength(1000)]
    public string ShippingAddress { get; set; }

    [Required]
    [StringLength(1000)]
    public string BillingAddress { get; set; }
} 