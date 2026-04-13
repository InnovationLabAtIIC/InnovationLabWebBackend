using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Categories;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class CategoriesController(IRepository<LandingDbContext, Category> categoryRepo) : ControllerBase
{
    private readonly IRepository<LandingDbContext, Category> _categoryRepo = categoryRepo;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<ActionResult<IList<CategoryResponseDto>>> GetCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var categories = await _categoryRepo.GetAsync(skip, pageSize);
        var categoryDtos = categories.Adapt<IList<CategoryResponseDto>>();
        return Ok(categoryDtos);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetCategoryById))]
    public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        var dto = category.Adapt<CategoryResponseDto>();
        return Ok(dto);
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateCategory))]
    public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryCreateDto createDto)
    {
        var category = createDto.Adapt<Category>();
        await _categoryRepo.AddAsync(category);
        await _categoryRepo.SaveChangesAsync();

        var dto = category.Adapt<CategoryResponseDto>();
        return CreatedAtAction(nameof(GetCategoryById), new { id = dto.Id }, dto);
    }

    [Authorize]
    [HttpPut("{id:guid}", Name = nameof(UpdateCategory))]
    public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] CategoryUpdateDto updateDto)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        updateDto.Adapt(category);
        _categoryRepo.Update(category);
        await _categoryRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}", Name = nameof(DeleteCategory))]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        _categoryRepo.HardDelete(category);
        await _categoryRepo.SaveChangesAsync();
        return NoContent();
    }
}
