using System;
using System.Collections.Generic;
using EcommercePlatform.Products;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Shops;

public class Shop : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string LogoUrl { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string BusinessHours { get; set; }
    public string TaxId { get; set; }
    public string PaymentMethods { get; set; }
    public string ShippingMethods { get; set; }
    public ICollection<Product> Products { get; set; }

    protected Shop()
    {
        Products = new List<Product>();
    }

    public Shop(
        Guid id,
        string name,
        string description,
        string logoUrl,
        string address,
        string phoneNumber,
        string email,
        string businessHours,
        string taxId,
        string paymentMethods,
        string shippingMethods,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        LogoUrl = logoUrl;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        IsActive = true;
        BusinessHours = businessHours;
        TaxId = taxId;
        PaymentMethods = paymentMethods;
        ShippingMethods = shippingMethods;
        Products = new List<Product>();
    }

    public void UpdateContactInfo(string phoneNumber, string email)
    {
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void UpdateBusinessInfo(string businessHours, string taxId)
    {
        BusinessHours = businessHours;
        TaxId = taxId;
    }

    public void UpdateShippingAndPayment(string paymentMethods, string shippingMethods)
    {
        PaymentMethods = paymentMethods;
        ShippingMethods = shippingMethods;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
} 