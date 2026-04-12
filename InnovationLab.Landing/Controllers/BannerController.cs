using AutoMapper;
using InnovationLab.Shared.Enums;
using InnovationLab.Shared.Interfaces;
using InnovationLabBackend.Api.Dtos.Banners;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]

    [Route("api/v1/[controller]")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerRepo bannerRepo;
        private readonly IMediaService mediaService;
        private readonly IMapper mapper;

        public BannerController(IBannerRepo bannerRepo, IMediaService mediaService, IMapper mapper)
        {
            this.bannerRepo = bannerRepo;
            this.mediaService = mediaService;
            this.mapper = mapper;
        }

        [HttpPost(Name = "CreateBanner")]
        public async Task<ActionResult<BannerDTO>> CreateBanner([FromForm] CreateBannerDTO bannerDto)
        {
            if (bannerDto.Image == null)
            {
                return BadRequest("Banner image is required.");
            }

            string mediaTypeString = bannerDto.Image.ContentType.ToLower();
            MediaType mediaType = mediaTypeString.StartsWith("image") ? MediaType.Image
                : mediaTypeString.StartsWith("video") ? MediaType.Video
                : MediaType.NotSupported;

            if (mediaType == MediaType.NotSupported)
            {
                return StatusCode(
                    StatusCodes.Status415UnsupportedMediaType,
                    "Unsupported file format. Only image and video files are supported."
                );
            }

            string? imageUrl = await mediaService.UploadAsync(
                file: bannerDto.Image,
                mediaType: mediaType,
                folder: "banners"
            );

            if (imageUrl == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while uploading the media.");
            }

            var newBanner = mapper.Map<Banner>(bannerDto);
            newBanner.Url = imageUrl;

            var createdBanner = await bannerRepo.CreateBannerAsync(newBanner);
            var createdBannerDto = mapper.Map<BannerDTO>(createdBanner);

            return CreatedAtAction(nameof(GetAllBanners), new { id = createdBanner.Id }, createdBannerDto);
        }

        [HttpGet(Name = "GetAllBanners")]
        public async Task<ActionResult<IEnumerable<BannerGetDTO>>> GetAllBanners(
            [FromQuery] MediaType? type = null,
            [FromQuery] DateTimeOffset? startDate = null,
            [FromQuery] DateTimeOffset? endDate = null,
            [FromQuery] DateTimeOffset? createdAfter = null)
        {
            var result = await bannerRepo.GetAllBannerAsync(type, startDate, endDate, createdAfter);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BannerGetDTO>> GetBannerById(Guid id)
        {
            var result = await bannerRepo.GetBannerByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Banner with ID  not found." });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBannerById(Guid id)
        {
            await bannerRepo.DeleteBannerAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateBanner([FromRoute] Guid id, [FromForm] BannerUpdateDTO banner)
        {
            if (banner == null)
            {
                return BadRequest();
            }
            var updatedBanner = await bannerRepo.updateBannerAsync(id, banner);
            if (updatedBanner == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateBanner([FromRoute] Guid id)
        {
            try
            {
                await bannerRepo.ActivateBanner(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Banner with id {id} not found." + ex);
            }
        }

        [HttpPut("{id}/schedule")]
        public async Task<ActionResult> ScheduleDateBanner([FromRoute] Guid id, [FromBody] DateScheduleDTO dateScheduleDTO)
        {
            try
            {
                await bannerRepo.ScheduleBannerDate(id, dateScheduleDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Banner with id {id} not found." + ex);
            }
        }
    }
}
