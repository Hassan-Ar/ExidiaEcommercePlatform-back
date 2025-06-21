using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Categories.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EcommercePlatform.Categories;

public interface ICategoryAppService :
    ICrudAppService<
        CategoryDto,
        Guid,
        GetCategoryListInput,
        CreateUpdateCategoryDto>
{
    Task<List<CategoryDto>> GetSubCategoriesAsync(Guid parentId);
    Task<CategoryDto> AddSubCategoryAsync(Guid parentId, CreateUpdateCategoryDto input);
    Task<CategoryDto> RemoveSubCategoryAsync(Guid parentId, Guid subCategoryId);
    Task<CategoryDto> ToggleActiveStatusAsync(Guid id);
    /// <summary>
    /// Returns the top active categories ordered by <see cref="CategoryDto.DisplayOrder"/>.
    /// </summary>
    /// <param name="maxCount">Maximum number of categories to return. Defaults to 6.</param>
    Task<List<CategoryDto>> GetTopAsync(int maxCount = 6);

    /// <summary>
    /// Returns a lightweight list of active categories for lookup purposes (id and name only).
    /// </summary>
    Task<List<CategoryLookupDto>> GetLookupAsync();
} 