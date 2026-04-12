using AutoMapper;
using InnovationLabBackend.Api.Dtos.Faqs;
using InnovationLabBackend.Api.Interfaces;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]
    [Route("api/v1/faqs")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class FaqsController(IFaqsRepo faqsRepo, IMapper mapper, ICategoriesRepo categoriesRepo) : ControllerBase
    {
        private readonly IFaqsRepo _faqsRepo = faqsRepo;
        private readonly IMapper _mapper = mapper;
        private readonly ICategoriesRepo _categoriesRepo = categoriesRepo;

        [HttpGet]
        public async Task<ActionResult<PaginatedFaqResponseDto>> GetFaqs(
            [FromQuery] string? category,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery(Name = "sort_by")] string sortBy = "created_at",
            [FromQuery(Name = "sort_order")] string sortOrder = "desc")
        {
            var faqs = await _faqsRepo.GetFaqsAsync(category, sortBy, sortOrder, page, limit);

            var totalItems = await _faqsRepo.GetTotalCountAsync(category);

            var faqDtos = _mapper.Map<List<FaqResponseDto>>(faqs);

            var response = new PaginatedFaqResponseDto
            {
                Page = page,
                Limit = limit,
                TotalItems = totalItems,
                Data = faqDtos
            };

            return Ok(response);
        }



        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FaqResponseDto>> GetFaqById(Guid id)
        {
            var faq = await _faqsRepo.GetFaqByIdAsync(id);
            if (faq == null)
                return NotFound("FAQ with the provided ID does not exist.");

            return Ok(_mapper.Map<FaqResponseDto>(faq));
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FaqResponseDto>> CreateFaq([FromBody] CreateFaqDto createFaqDto)
        {
            var category = await _categoriesRepo.GetCategoryByIdAsync(createFaqDto.CategoryId);
            if (category == null)
                return BadRequest("Invalid category ID.");

            var faq = _mapper.Map<Faq>(createFaqDto);
            faq.Category = category;

            var createdFaq = await _faqsRepo.CreateFaqAsync(faq);
            var responseDto = _mapper.Map<FaqResponseDto>(createdFaq);

            return CreatedAtAction(nameof(GetFaqById), new { id = createdFaq.Id }, responseDto);
        }


        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<FaqResponseDto>> UpdateFaq(Guid id, [FromBody] UpdateFaqDto updateFaqDto)
        {
            var faq = await _faqsRepo.GetFaqByIdAsync(id);
            if (faq == null)
                return NotFound("FAQ with the provided ID does not exist.");

            var category = await _categoriesRepo.GetCategoryByIdAsync(updateFaqDto.CategoryId);
            if (category == null)
                return BadRequest("Invalid category ID.");

            _mapper.Map(updateFaqDto, faq);
            faq.Category = category;

            var updatedFaq = await _faqsRepo.UpdateFaqAsync(faq);
            var responseDto = _mapper.Map<FaqResponseDto>(updatedFaq);

            return Ok(responseDto);
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteFaq(Guid id)
        {
            var faq = await _faqsRepo.GetFaqByIdAsync(id);
            if (faq == null)
                return NotFound("FAQ with the provided ID does not exist.");

            var result = await _faqsRepo.DeleteFaqAsync(faq);
            if (!result)
                return StatusCode(500, "An error occurred while deleting the FAQ.");

            return Ok("FAQ deleted successfully.");
        }
    }
}
