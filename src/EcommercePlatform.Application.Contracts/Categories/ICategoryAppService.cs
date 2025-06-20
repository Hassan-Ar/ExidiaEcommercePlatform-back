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
} 