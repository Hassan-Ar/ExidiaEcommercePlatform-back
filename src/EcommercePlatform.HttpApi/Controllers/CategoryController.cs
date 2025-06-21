using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Categories;
using EcommercePlatform.Categories.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.Controllers;

[Route("api/app/category")]
[AllowAnonymous]
public class CategoryController : EcommercePlatformController
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoryController(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoryListInput input)
    {
        return _categoryAppService.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<CategoryDto> GetAsync(Guid id)
    {
        return _categoryAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<CategoryDto> CreateAsync([FromForm] CreateUpdateCategoryDto input)
    {
        return _categoryAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<CategoryDto> UpdateAsync(Guid id, [FromForm] CreateUpdateCategoryDto input)
    {
        return _categoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _categoryAppService.DeleteAsync(id);
    }

    [HttpGet("{parentId}/subcategories")]
    public virtual Task<List<CategoryDto>> GetSubCategoriesAsync(Guid parentId)
    {
        return _categoryAppService.GetSubCategoriesAsync(parentId);
    }

    [HttpPost("{parentId}/subcategories")]
    public virtual Task<CategoryDto> AddSubCategoryAsync(Guid parentId, [FromForm] CreateUpdateCategoryDto input)
    {
        return _categoryAppService.AddSubCategoryAsync(parentId, input);
    }

    [HttpDelete("{parentId}/subcategories/{subCategoryId}")]
    public virtual Task<CategoryDto> RemoveSubCategoryAsync(Guid parentId, Guid subCategoryId)
    {
        return _categoryAppService.RemoveSubCategoryAsync(parentId, subCategoryId);
    }

    [HttpPost("{id}/toggle-active-status")]
    public virtual Task<CategoryDto> ToggleActiveStatusAsync(Guid id)
    {
        return _categoryAppService.ToggleActiveStatusAsync(id);
    }

    [HttpGet("top")]
    public virtual Task<List<CategoryDto>> GetTopAsync([FromQuery] int maxCount = 6)
    {
        return _categoryAppService.GetTopAsync(maxCount);
    }

    [HttpGet("lookup")]
    public virtual Task<List<CategoryLookupDto>> GetLookupAsync()
    {
        return _categoryAppService.GetLookupAsync();
    }
}