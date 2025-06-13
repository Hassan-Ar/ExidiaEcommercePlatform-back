using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace EcommercePlatform.Carts;

public class Cart : FullAuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public ICollection<CartItem> Items { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
    }
} 