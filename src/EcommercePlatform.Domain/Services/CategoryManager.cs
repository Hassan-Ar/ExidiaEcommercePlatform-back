using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Categories;
using EcommercePlatform.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// Domain service for managing categories.
    /// </summary>
    public class CategoryManager : DomainService, ICategoryManager
    {
        private readonly IRepository<Category, Guid> _categoryRepository;

        /// <summary>
        /// Creates a new instance of CategoryManager.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryManager(IRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <param name="description">The description of the category.</param>
        /// <param name="parentCategoryId">The ID of the parent category, if any.</param>
        /// <returns>The newly created category.</returns>
        public async Task<Category> CreateAsync(
            string name,
            string description = null,
            Guid? parentCategoryId = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(name));
            }

            // Check if parent category exists
            if (parentCategoryId.HasValue)
            {
                if (!await _categoryRepository.AnyAsync(c => c.Id == parentCategoryId.Value))
                {
                    throw new ArgumentException("Parent category does not exist.", nameof(parentCategoryId));
                }
            }

            // Create category
            var category = new Category(
                GuidGenerator.Create(),
                name,
                description,
                imageUrl:null,
                parentCategoryId);

            return await _categoryRepository.InsertAsync(category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="name">The new name of the category.</param>
        /// <param name="description">The new description of the category.</param>
        /// <param name="parentCategoryId">The new parent category ID.</param>
        /// <param name="displayOrder">The new display order.</param>
        /// <param name="imageUrl">The new image URL.</param>
        /// <returns>The updated category.</returns>
        public async Task<Category> UpdateAsync(
            Guid id,
            string name,
            string description = null,
            Guid? parentCategoryId = null,
            int displayOrder = 0,
            string imageUrl = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(name));
            }

            // Get category
            var category = await _categoryRepository.GetAsync(id);

            // Check if parent category exists and is not the category itself
            if (parentCategoryId.HasValue)
            {
                if (parentCategoryId.Value == id)
                {
                    throw new ArgumentException("A category cannot be its own parent.", nameof(parentCategoryId));
                }

                if (!await _categoryRepository.AnyAsync(c => c.Id == parentCategoryId.Value))
                {
                    throw new ArgumentException("Parent category does not exist.", nameof(parentCategoryId));
                }

                // Check for circular reference
                await CheckForCircularReferenceAsync(parentCategoryId.Value, id);
            }

            // Update category
           // category.Update(name, description, parentCategoryId, displayOrder, imageUrl);

            return await _categoryRepository.UpdateAsync(category);
        }

        /// <summary>
        /// Activates a category.
        /// </summary>
        /// <param name="id">The ID of the category to activate.</param>
        /// <returns>The activated category.</returns>
        public async Task<Category> ActivateAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            category.Activate();
            return await _categoryRepository.UpdateAsync(category);
        }

        /// <summary>
        /// Deactivates a category.
        /// </summary>
        /// <param name="id">The ID of the category to deactivate.</param>
        /// <returns>The deactivated category.</returns>
        public async Task<Category> DeactivateAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            category.Deactivate();
            return await _categoryRepository.UpdateAsync(category);
        }

        /// <summary>
        /// Checks for circular references in the category hierarchy.
        /// </summary>
        /// <param name="parentId">The parent category ID to check.</param>
        /// <param name="childId">The child category ID to check.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CheckForCircularReferenceAsync(Guid parentId, Guid childId)
        {
            var visited = new HashSet<Guid>();
            var currentId = parentId;

            while (currentId != Guid.Empty)
            {
                if (visited.Contains(currentId))
                {
                    throw new InvalidOperationException("Circular reference detected in category hierarchy.");
                }

                if (currentId == childId)
                {
                    throw new InvalidOperationException("Setting this parent would create a circular reference.");
                }

                visited.Add(currentId);

                var parent = await _categoryRepository.FindAsync(currentId);
                if (parent == null || !parent.ParentCategoryId.HasValue)
                {
                    break;
                }

                currentId = parent.ParentCategoryId.Value;
            }
        }
    }

    /// <summary>
    /// Interface for the category manager domain service.
    /// </summary>
    public interface ICategoryManager : IDomainService
    {
        /// <summary>
        /// Creates a new category.
        /// </summary>
        Task<Category> CreateAsync(
            string name,
            string description = null,
            Guid? parentCategoryId = null);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        Task<Category> UpdateAsync(
            Guid id,
            string name,
            string description = null,
            Guid? parentCategoryId = null,
            int displayOrder = 0,
            string imageUrl = null);

        /// <summary>
        /// Activates a category.
        /// </summary>
        Task<Category> ActivateAsync(Guid id);

        /// <summary>
        /// Deactivates a category.
        /// </summary>
        Task<Category> DeactivateAsync(Guid id);
    }
}
