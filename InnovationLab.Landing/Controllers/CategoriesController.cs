using AutoMapper;
using InnovationLabBackend.Api.Dtos.Categories;
using InnovationLabBackend.Api.Interfaces;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class CategoriesController(ICategoriesRepo categoriesRepo, IMapper mapper) : ControllerBase
    {
        private readonly ICategoriesRepo _categoriesRepo = categoriesRepo;
        private readonly IMapper _mapper = mapper;

        [HttpGet(Name = "GetAllCategories")]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _categoriesRepo.GetAllCategoriesAsync();
            var categoryDtos = _mapper.Map<List<CategoryResponseDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id:guid}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(Guid id)
        {
            var category = await _categoriesRepo.GetCategoryByIdAsync(id);
            if (category is null)
                return NotFound("Category with provided Id doesn't exist.");

            var dto = _mapper.Map<CategoryResponseDto>(category);
            return Ok(dto);
        }

        [HttpPost(Name = "CreateCategory")]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryDto createDto)
        {
            var newCategory = _mapper.Map<Category>(createDto);
            var created = await _categoriesRepo.CreateCategoryAsync(newCategory);
            var dto = _mapper.Map<CategoryResponseDto>(created);
            return Ok(dto);
        }

        [HttpPut("{id:guid}", Name = "UpdateCategory")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto updateDto)
        {
            var category = await _categoriesRepo.GetCategoryByIdAsync(id);
            if (category is null)
                return NotFound("Category with provided Id doesn't exist.");

            _mapper.Map(updateDto, category);
            var updated = await _categoriesRepo.UpdateCategoryAsync(category);
            var dto = _mapper.Map<CategoryResponseDto>(updated);
            return Ok(dto);
        }

        [HttpDelete("{id:guid}", Name = "DeleteCategoryById")]
        public async Task<IActionResult> DeleteCategoryById(Guid id)
        {
            var category = await _categoriesRepo.GetCategoryByIdAsync(id);
            if (category is null)
                return NotFound("Category with provided Id doesn't exist.");

            var deleted = await _categoriesRepo.DeleteCategoryAsync(category);
            if (!deleted)
                return StatusCode(500, "Internal server error during deletion.");

            return Ok("Category deleted successfully.");
        }
    }
}
