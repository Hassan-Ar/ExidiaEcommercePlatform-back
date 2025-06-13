using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents a category in the e-commerce platform.
    /// </summary>
    public class Category : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this category belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The ID of the parent category, if any.
        /// </summary>
        public Guid? ParentCategoryId { get; set; }

        /// <summary>
        /// The display order of the category.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// The URL-friendly slug for the category.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// The URL to the category's image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Indicates whether the category is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Creates a new Category instance.
        /// </summary>
        protected Category()
        {
        }

        /// <summary>
        /// Creates a new Category instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the category.</param>
        /// <param name="tenantId">The tenant ID that this category belongs to.</param>
        /// <param name="name">The name of the category.</param>
        /// <param name="description">The description of the category.</param>
        /// <param name="parentCategoryId">The ID of the parent category, if any.</param>
        public Category(
            Guid id,
            Guid? tenantId,
            string name,
            string description = null,
            Guid? parentCategoryId = null)
            : base(id)
        {
            TenantId = tenantId;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            DisplayOrder = 0;
            IsActive = true;
            Slug = GenerateSlug(name);
        }

        /// <summary>
        /// Generates a URL-friendly slug from the specified name.
        /// </summary>
        /// <param name="name">The name to generate a slug from.</param>
        /// <returns>A URL-friendly slug.</returns>
        private string GenerateSlug(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            // Replace spaces with hyphens and convert to lowercase
            return name.Replace(" ", "-").ToLower();
        }

        /// <summary>
        /// Updates the category's information.
        /// </summary>
        /// <param name="name">The new name of the category.</param>
        /// <param name="description">The new description of the category.</param>
        /// <param name="parentCategoryId">The new parent category ID.</param>
        /// <param name="displayOrder">The new display order.</param>
        /// <param name="imageUrl">The new image URL.</param>
        public void Update(
            string name,
            string description = null,
            Guid? parentCategoryId = null,
            int displayOrder = 0,
            string imageUrl = null)
        {
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            DisplayOrder = displayOrder;
            ImageUrl = imageUrl;
            Slug = GenerateSlug(name);
        }

        /// <summary>
        /// Activates the category.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the category.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
