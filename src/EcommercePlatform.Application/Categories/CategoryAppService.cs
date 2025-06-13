using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Categories;
using EcommercePlatform.Categories.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EcommercePlatform.Categories;

[Authorize]
public class CategoryAppService :
    CrudAppService<
        Category,
        CategoryDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDto>,
    ICategoryAppService
{
    private readonly IRepository<Category, Guid> _categoryRepository;

    public CategoryAppService(IRepository<Category, Guid> repository)
        : base(repository)
    {
        _categoryRepository = repository;
    }

    public async Task<List<CategoryDto>> GetSubCategoriesAsync(Guid parentId)
    {
        var categories = await _categoryRepository.GetListAsync(c => c.ParentCategoryId == parentId);
        return ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> AddSubCategoryAsync(Guid parentId, CreateUpdateCategoryDto input)
    {
        var parentCategory = await _categoryRepository.GetAsync(parentId);
        var subCategory = ObjectMapper.Map<CreateUpdateCategoryDto, Category>(input);
        parentCategory.AddSubCategory(subCategory);
        await _categoryRepository.InsertAsync(subCategory);
        return ObjectMapper.Map<Category, CategoryDto>(subCategory);
    }

    public async Task<CategoryDto> RemoveSubCategoryAsync(Guid parentId, Guid subCategoryId)
    {
        var parentCategory = await _categoryRepository.GetAsync(parentId);
        var subCategory = await _categoryRepository.GetAsync(subCategoryId);
        parentCategory.RemoveSubCategory(subCategory);
        await _categoryRepository.UpdateAsync(subCategory);
        return ObjectMapper.Map<Category, CategoryDto>(subCategory);
    }

    public async Task<CategoryDto> ToggleActiveStatusAsync(Guid id)
    {
        var category = await _categoryRepository.GetAsync(id);
        if (category.IsActive)
        {
            category.Deactivate();
        }
        else
        {
            category.Activate();
        }
        await _categoryRepository.UpdateAsync(category);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }
} 