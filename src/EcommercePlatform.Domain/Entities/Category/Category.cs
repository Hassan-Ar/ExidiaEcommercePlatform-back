using System;
using System.Collections.Generic;
using EcommercePlatform.Products;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Categories;

public class Category : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }
    public ICollection<Product> Products { get; set; }

    protected Category()
    {
        SubCategories = new List<Category>();
        Products = new List<Product>();
    }

    public Category(
        Guid id,
        string name,
        string description,
        string imageUrl,
        Guid? parentCategoryId = null,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        IsActive = true;
        ParentCategoryId = parentCategoryId;
        SubCategories = new List<Category>();
        Products = new List<Product>();
    }

    public void AddSubCategory(Category subCategory)
    {
        if (subCategory.ParentCategoryId.HasValue)
        {
            throw new InvalidOperationException("Category already has a parent");
        }
        subCategory.ParentCategoryId = Id;
        SubCategories.Add(subCategory);
    }

    public void RemoveSubCategory(Category subCategory)
    {
        if (subCategory.ParentCategoryId != Id)
        {
            throw new InvalidOperationException("Category is not a subcategory of this category");
        }
        subCategory.ParentCategoryId = null;
        SubCategories.Remove(subCategory);
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