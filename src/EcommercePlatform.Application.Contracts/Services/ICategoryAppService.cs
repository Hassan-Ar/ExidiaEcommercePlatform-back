//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Volo.Abp.Application.Dtos;
//using Volo.Abp.Application.Services;

//namespace EcommercePlatform.Application.Contracts.Services
//{
//    /// <summary>
//    /// Interface for the category application service.
//    /// </summary>
//    public interface ICategoryAppService : IApplicationService
//    {
//        /// <summary>
//        /// Gets a category by ID.
//        /// </summary>
//        Task<CategoryDto> GetAsync(Guid id);

//        /// <summary>
//        /// Gets a list of all categories.
//        /// </summary>
//        Task<List<CategoryDto>> GetListAsync();

//        /// <summary>
//        /// Gets a paged list of categories.
//        /// </summary>
//        Task<PagedResultDto<CategoryDto>> GetPagedListAsync(PagedAndSortedResultRequestDto input);

//        /// <summary>
//        /// Gets a list of root categories (categories without a parent).
//        /// </summary>
//        Task<List<CategoryDto>> GetRootCategoriesAsync();

//        /// <summary>
//        /// Gets a list of child categories for a given parent category.
//        /// </summary>
//        Task<List<CategoryDto>> GetChildCategoriesAsync(Guid parentId);

//        /// <summary>
//        /// Creates a new category.
//        /// </summary>
//        Task<CategoryDto> CreateAsync(CreateCategoryDto input);

//        /// <summary>
//        /// Updates an existing category.
//        /// </summary>
//        Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto input);

//        /// <summary>
//        /// Deletes a category.
//        /// </summary>
//        Task DeleteAsync(Guid id);

//        /// <summary>
//        /// Activates a category.
//        /// </summary>
//        Task<CategoryDto> ActivateAsync(Guid id);

//        /// <summary>
//        /// Deactivates a category.
//        /// </summary>
//        Task<CategoryDto> DeactivateAsync(Guid id);
//    }

//    /// <summary>
//    /// DTO for category data.
//    /// </summary>
//    public class CategoryDto : EntityDto<Guid>
//    {
//        /// <summary>
//        /// The name of the category.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the category.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The ID of the parent category, if any.
//        /// </summary>
//        public Guid? ParentCategoryId { get; set; }

//        /// <summary>
//        /// The display order of the category.
//        /// </summary>
//        public int DisplayOrder { get; set; }

//        /// <summary>
//        /// The URL-friendly slug for the category.
//        /// </summary>
//        public string Slug { get; set; }

//        /// <summary>
//        /// The URL to the category's image.
//        /// </summary>
//        public string ImageUrl { get; set; }

//        /// <summary>
//        /// Indicates whether the category is active.
//        /// </summary>
//        public bool IsActive { get; set; }

//        /// <summary>
//        /// The creation time of the category.
//        /// </summary>
//        public DateTime CreationTime { get; set; }
//    }

//    /// <summary>
//    /// DTO for creating a category.
//    /// </summary>
//    public class CreateCategoryDto
//    {
//        /// <summary>
//        /// The name of the category.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the category.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The ID of the parent category, if any.
//        /// </summary>
//        public Guid? ParentCategoryId { get; set; }
//    }

//    /// <summary>
//    /// DTO for updating a category.
//    /// </summary>
//    public class UpdateCategoryDto
//    {
//        /// <summary>
//        /// The name of the category.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the category.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The ID of the parent category, if any.
//        /// </summary>
//        public Guid? ParentCategoryId { get; set; }

//        /// <summary>
//        /// The display order of the category.
//        /// </summary>
//        public int DisplayOrder { get; set; }

//        /// <summary>
//        /// The URL to the category's image.
//        /// </summary>
//        public string ImageUrl { get; set; }
//    }
//} 