using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InnovationLabBackend.Api.Dtos.Testimonials;
using InnovationLabBackend.Api.Interfaces;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]
    //[Consumes("application/json")]
    //[Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class TestimonialsController(ITestimonialsRepo testimonialsRepo, IMapper mapper) : ControllerBase
    {
        private readonly ITestimonialsRepo _testimonialsRepo = testimonialsRepo;
        private readonly IMapper _mapper = mapper;

        [HttpGet(Name = "GetTestimonials")]
        public async Task<ActionResult<List<TestimonialResponseDto>>> GetTestimonials()
        {
            var testimonials = await _testimonialsRepo.GetTestimonialsAsync();
            var testimonialsDto = _mapper.Map<List<TestimonialResponseDto>>(testimonials);
            return Ok(testimonialsDto);
        }

        [HttpGet("{id}", Name = "GetTestimonialById")]
        public async Task<ActionResult<TestimonialResponseDto>> GetTestimonialById(Guid id)
        {
            var testimonial = await _testimonialsRepo.GetTestimonialByIdAsync(id);

            if (testimonial == null)
            {
                return NotFound(new
                {
                    Message = "Testinomial Not Found"
                });
            }

            var testimonialDto = _mapper.Map<TestimonialResponseDto>(testimonial);
            return Ok(testimonialDto);
        }

        [Authorize]
        [HttpPost(Name = "CreateTestimonial")]
        public async Task<ActionResult<TestimonialResponseDto>> CreateTestimonial([FromForm] CreateTestimonialDto testimonialCreateDto)
        {
            string? imageUrl = null;
            if (testimonialCreateDto.ImageUrl != null && testimonialCreateDto.ImageUrl.Length > 0)
            {
                imageUrl = await UploadImage(testimonialCreateDto.ImageUrl);
                if (imageUrl == null)
                {
                    return BadRequest("Image upload failed");
                }
            }
            var newTestimonial = _mapper.Map<Testimonial>(testimonialCreateDto);
            newTestimonial.ImageUrl = imageUrl;
            var createdTestimonial = await _testimonialsRepo.CreateTestimonialAsync(newTestimonial);

            return CreatedAtAction(nameof(GetTestimonials), new { id = createdTestimonial.Id }, createdTestimonial);
        }

        [HttpPatch("{id}", Name = "UpdateTestimonial")]
        public async Task<ActionResult> UpdateTestimonial(Guid id, [FromForm] UpdateTestimonialDto testimonialUpdateDto)
        {
            if (testimonialUpdateDto == null)
            {
                return BadRequest("Invalid data");
            }

            var testimonial = await _testimonialsRepo.GetTestimonialByIdAsync(id);

            if (testimonial == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (testimonialUpdateDto.Name != null)
                testimonial.Name = testimonialUpdateDto.Name;

            if (testimonialUpdateDto.Text != null)
                testimonial.Text = testimonialUpdateDto.Text;

            if (testimonialUpdateDto.Designation != null)
                testimonial.Designation = testimonialUpdateDto.Designation;

            if (testimonialUpdateDto.Organization != null)
                testimonial.Organization = testimonialUpdateDto.Organization;

            //_mapper.Map(testimonialUpdateDto, testimonial);
            if (testimonialUpdateDto.ImageUrl != null && testimonialUpdateDto.ImageUrl.Length > 0)
            {
                //delete garne function banara handle garna xa
                var imageUrl = await UploadImage(testimonialUpdateDto.ImageUrl);
                if (imageUrl == null)
                {
                    return BadRequest("Image upload failed");
                }
                testimonial.ImageUrl = imageUrl;
            }
            await _testimonialsRepo.UpdateTestimonialAsync(testimonial);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteTestimonial")]
        public async Task<ActionResult> DeleteTestimonial(Guid id)
        {
            var testimonial = await _testimonialsRepo.GetTestimonialByIdAsync(id);

            if (testimonial == null)
            {
                return NotFound();
            }

            await _testimonialsRepo.DeleteTestimonialAsync(testimonial);

            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<string?> UploadImage(IFormFile file)
        {
            var cloudinary = new Cloudinary();
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill"),
                PublicId = $"testimonials/{Guid.NewGuid()}"
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                return null;
            }

            return uploadResult.SecureUrl.AbsoluteUri;
        }


    }
}
