using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Testimonials;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using SharedMediaType = InnovationLab.Shared.Enums.MediaType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class TestimonialsController(
    IRepository<LandingDbContext, Testimonial> testimonialRepo,
    IMediaService mediaService
) : ControllerBase
{
    private const string TestimonialsFolder = "testimonials";

    private readonly IRepository<LandingDbContext, Testimonial> _testimonialRepo = testimonialRepo;
    private readonly IMediaService _mediaService = mediaService;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetTestimonials))]
    public async Task<ActionResult<IList<TestimonialResponseDto>>> GetTestimonials([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var testimonials = await _testimonialRepo.GetAsync(skip, pageSize);
        var testimonialDtos = testimonials.Adapt<IList<TestimonialResponseDto>>();
        return Ok(testimonialDtos);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetTestimonialById))]
    public async Task<ActionResult<TestimonialResponseDto>> GetTestimonialById(Guid id)
    {
        var testimonial = await _testimonialRepo.GetByIdAsync(id);
        if (testimonial is null)
        {
            return NotFound();
        }

        return Ok(testimonial.Adapt<TestimonialResponseDto>());
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateTestimonial))]
    public async Task<ActionResult<TestimonialResponseDto>> CreateTestimonial([FromForm] TestimonialCreateDto testimonialCreateDto)
    {
        var testimonial = testimonialCreateDto.Adapt<Testimonial>();

        if (testimonialCreateDto.ImageUrl is not null && testimonialCreateDto.ImageUrl.Length > 0)
        {
            var imageUrl = await _mediaService.UploadAsync(testimonialCreateDto.ImageUrl, SharedMediaType.Image, TestimonialsFolder);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            testimonial.ImageUrl = imageUrl;
        }

        await _testimonialRepo.AddAsync(testimonial);
        await _testimonialRepo.SaveChangesAsync();

        var response = testimonial.Adapt<TestimonialResponseDto>();
        return CreatedAtAction(nameof(GetTestimonialById), new { id = response.Id }, response);
    }

    [Authorize]
    [HttpPatch("{id:guid}", Name = nameof(UpdateTestimonial))]
    public async Task<ActionResult> UpdateTestimonial(Guid id, [FromForm] TestimonialUpdateDto testimonialUpdateDto)
    {
        var testimonial = await _testimonialRepo.GetByIdAsync(id);
        if (testimonial is null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(testimonialUpdateDto.Name))
        {
            testimonial.Name = testimonialUpdateDto.Name;
        }

        if (!string.IsNullOrWhiteSpace(testimonialUpdateDto.Text))
        {
            testimonial.Text = testimonialUpdateDto.Text;
        }

        if (!string.IsNullOrWhiteSpace(testimonialUpdateDto.Designation))
        {
            testimonial.Designation = testimonialUpdateDto.Designation;
        }

        if (!string.IsNullOrWhiteSpace(testimonialUpdateDto.Organization))
        {
            testimonial.Organization = testimonialUpdateDto.Organization;
        }

        if (testimonialUpdateDto.ImageUrl is not null && testimonialUpdateDto.ImageUrl.Length > 0)
        {
            var imageUrl = await _mediaService.UploadAsync(testimonialUpdateDto.ImageUrl, SharedMediaType.Image, TestimonialsFolder);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            testimonial.ImageUrl = imageUrl;
        }

        _testimonialRepo.Update(testimonial);
        await _testimonialRepo.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}", Name = nameof(DeleteTestimonial))]
    public async Task<ActionResult> DeleteTestimonial(Guid id)
    {
        var testimonial = await _testimonialRepo.GetByIdAsync(id);
        if (testimonial is null)
        {
            return NotFound();
        }

        _testimonialRepo.HardDelete(testimonial);
        await _testimonialRepo.SaveChangesAsync();
        return NoContent();
    }
}
