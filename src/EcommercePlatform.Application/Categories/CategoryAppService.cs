using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.BlobStoring;
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
        EcommercePlatform.Categories.Dtos.GetCategoryListInput,
        CreateUpdateCategoryDto>,
    ICategoryAppService
{
    private readonly IRepository<Category, Guid> _categoryRepository;
    private readonly IBlobStorageService _blobStorageService;

    public CategoryAppService(
        IRepository<Category, Guid> repository,
        IBlobStorageService blobStorageService)
        : base(repository)
    {
        _categoryRepository = repository;
        _blobStorageService = blobStorageService;
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

    public override async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
    {
        if (input == null) return null;

        var category = MapToEntity(input);

        if (input.Image != null)
        {
            var imageName = await _blobStorageService.SaveImageAsync(input.Image);
            category.ImageUrl = _blobStorageService.GetImageUrl(imageName);
        }

        await Repository.InsertAsync(category, autoSave: true);

        return MapToGetOutputDto(category);
    }

    public override async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
    {
        var category = await Repository.GetAsync(id);

        if (input.Image != null)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var oldImageName = category.ImageUrl.Split('/').Last();
                await _blobStorageService.DeleteImageAsync(oldImageName);
            }

            var imageName = await _blobStorageService.SaveImageAsync(input.Image);
            category.ImageUrl = _blobStorageService.GetImageUrl(imageName);
        }

        MapToEntity(input, category);
        await Repository.UpdateAsync(category);

        return MapToGetOutputDto(category);
    }

    public override async Task DeleteAsync(Guid id)
    {
        var category = await Repository.GetAsync(id);

        // Delete image if exists
        if (!string.IsNullOrEmpty(category.ImageUrl))
        {
            var imageName = category.ImageUrl.Split('/').Last();
            await _blobStorageService.DeleteImageAsync(imageName);
        }

        await base.DeleteAsync(id);
    }

    public override async Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoryListInput input)
    {
        var query = await _categoryRepository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(c => c.Name.Contains(input.Filter) || c.Description.Contains(input.Filter));
        }

        if (input.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == input.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            query = ApplySorting(query, input);
        }
        else
        {
            query = query.OrderBy(c => c.Name);
        }

        var totalCount = query.Count();
        var items = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        var dtoList = ObjectMapper.Map<List<Category>, List<CategoryDto>>(items);
        return new PagedResultDto<CategoryDto>(totalCount, dtoList);
    }
} 