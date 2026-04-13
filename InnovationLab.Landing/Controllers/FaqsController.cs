using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Faqs;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class FaqsController(
    IRepository<LandingDbContext, Faq> faqRepo,
    IRepository<LandingDbContext, Category> categoryRepo
) : ControllerBase
{
    private const string SortByCreatedAt = "created_at";
    private const string SortOrderDesc = "desc";

    private readonly IRepository<LandingDbContext, Faq> _faqRepo = faqRepo;
    private readonly IRepository<LandingDbContext, Category> _categoryRepo = categoryRepo;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetFaqs))]
    public async Task<ActionResult<FaqListResponseDto>> GetFaqs(
        [FromQuery] string? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery(Name = "sort_by")] string sortBy = SortByCreatedAt,
        [FromQuery(Name = "sort_order")] string sortOrder = SortOrderDesc)
    {
        var skip = (page - 1) * pageSize;
        var normalizedSortBy = sortBy.Trim().ToLowerInvariant();
        var normalizedSortOrder = sortOrder.Trim().ToLowerInvariant();

        var faqs = await _faqRepo.QueryAsync(query =>
        {
            var q = query.Include(f => f.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                var categoryFilter = category.Trim().ToLowerInvariant();
                q = q.Where(f => f.Category.Name.ToLower() == categoryFilter);
            }

            q = (normalizedSortBy, normalizedSortOrder) switch
            {
                (SortByCreatedAt, SortOrderDesc) => q.OrderByDescending(f => f.CreatedAt),
                (SortByCreatedAt, _) => q.OrderBy(f => f.CreatedAt),
                _ when normalizedSortOrder == SortOrderDesc => q.OrderByDescending(f => f.CreatedAt),
                _ => q.OrderBy(f => f.CreatedAt)
            };

            return q;
        }, skip, pageSize);

        var totalItems = await _faqRepo.CountAsync(f => string.IsNullOrWhiteSpace(category) || f.Category.Name.ToLower() == category.Trim().ToLowerInvariant());
        var faqDtos = faqs.Adapt<List<FaqResponseDto>>();
        var response = new FaqListResponseDto(page, pageSize, totalItems, faqDtos);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetFaqById))]
    public async Task<ActionResult<FaqResponseDto>> GetFaqById(Guid id)
    {
        var faq = (await _faqRepo.QueryAsync(q => q.Include(f => f.Category).Where(f => f.Id == id), 0, 1)).FirstOrDefault();
        if (faq is null)
        {
            return NotFound();
        }

        return Ok(faq.Adapt<FaqResponseDto>());
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateFaq))]
    public async Task<ActionResult<FaqResponseDto>> CreateFaq([FromBody] FaqCreateDto faqCreateDto)
    {
        var category = await _categoryRepo.GetByIdAsync(faqCreateDto.CategoryId);
        if (category is null)
        {
            return BadRequest();
        }

        var faq = faqCreateDto.Adapt<Faq>();
        faq.Category = category;

        await _faqRepo.AddAsync(faq);
        await _faqRepo.SaveChangesAsync();

        var response = faq.Adapt<FaqResponseDto>();
        return CreatedAtAction(nameof(GetFaqById), new { id = response.Id }, response);
    }

    [Authorize]
    [HttpPut("{id:guid}", Name = nameof(UpdateFaq))]
    public async Task<ActionResult> UpdateFaq(Guid id, [FromBody] FaqUpdateDto faqUpdateDto)
    {
        var faq = await _faqRepo.GetByIdAsync(id);
        if (faq is null)
        {
            return NotFound();
        }

        var category = await _categoryRepo.GetByIdAsync(faqUpdateDto.CategoryId);
        if (category is null)
        {
            return BadRequest();
        }

        faqUpdateDto.Adapt(faq);
        faq.Category = category;

        _faqRepo.Update(faq);
        await _faqRepo.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}", Name = nameof(DeleteFaq))]
    public async Task<ActionResult> DeleteFaq(Guid id)
    {
        var faq = await _faqRepo.GetByIdAsync(id);
        if (faq is null)
        {
            return NotFound();
        }

        _faqRepo.HardDelete(faq);
        await _faqRepo.SaveChangesAsync();
        return NoContent();
    }
}
