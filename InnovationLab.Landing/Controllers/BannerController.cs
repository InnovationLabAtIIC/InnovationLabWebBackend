using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Banners;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using InnovationLab.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InnovationLab.Shared.Extensions;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class BannerController(
    IRepository<LandingDbContext, Banner> bannerRepo,
    IMediaService mediaService
) : ControllerBase
{
    private const string BannerFolder = "banners";

    private readonly IRepository<LandingDbContext, Banner> _bannerRepo = bannerRepo;
    private readonly IMediaService _mediaService = mediaService;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetBanners))]
    public async Task<ActionResult<IList<BannerResponseDto>>> GetBanners(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] MediaType? type = null,
        [FromQuery] DateTimeOffset? scheduledStart = null,
        [FromQuery] DateTimeOffset? scheduledEnd = null,
        [FromQuery] DateTimeOffset? createdAfter = null)
    {
        var skip = (page - 1) * pageSize;

        var banners = await _bannerRepo.QueryAsync(query =>
        {
            var q = query.AsQueryable();

            if (type.HasValue)
            {
                q = q.Where(b => b.Type == type.Value);
            }

            if (scheduledStart.HasValue)
            {
                q = q.Where(b => b.ScheduledStart >= scheduledStart.Value);
            }

            if (scheduledEnd.HasValue)
            {
                q = q.Where(b => b.ScheduledEnd <= scheduledEnd.Value);
            }

            if (createdAfter.HasValue)
            {
                q = q.Where(b => b.CreatedAt >= createdAfter.Value);
            }

            return q.OrderByDescending(b => b.CreatedAt);
        }, skip, pageSize);

        var bannerDtos = banners.Adapt<IList<BannerResponseDto>>();
        return Ok(bannerDtos);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetBannerById))]
    public async Task<ActionResult<BannerResponseDto>> GetBannerById(Guid id)
    {
        var banner = await _bannerRepo.GetByIdAsync(id);
        if (banner is null)
        {
            return NotFound();
        }

        return Ok(banner.Adapt<BannerResponseDto>());
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateBanner))]
    public async Task<ActionResult<BannerResponseDto>> CreateBanner([FromForm] BannerCreateDto bannerCreateDto)
    {
        var mediaType = bannerCreateDto.Image.ContentType.ToMediaType();
        if (mediaType is MediaType.NotSupported)
        {
            return StatusCode(StatusCodes.Status415UnsupportedMediaType);
        }

        var imageUrl = await _mediaService.UploadAsync(
            file: bannerCreateDto.Image,
            mediaType: mediaType,
            folder: BannerFolder
        );

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var banner = bannerCreateDto.Adapt<Banner>();
        banner.Url = imageUrl;

        await _bannerRepo.AddAsync(banner);
        await _bannerRepo.SaveChangesAsync();

        var response = banner.Adapt<BannerResponseDto>();
        return CreatedAtAction(nameof(GetBannerById), new { id = response.Id }, response);
    }

    [Authorize]
    [HttpPatch("{id:guid}", Name = nameof(UpdateBanner))]
    public async Task<ActionResult> UpdateBanner(Guid id, [FromForm] BannerUpdateDto bannerUpdateDto)
    {
        var banner = await _bannerRepo.GetByIdAsync(id);
        if (banner is null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(bannerUpdateDto.Url))
        {
            banner.Url = bannerUpdateDto.Url;
        }

        if (bannerUpdateDto.Type.HasValue)
        {
            banner.Type = bannerUpdateDto.Type.Value;
        }

        if (!string.IsNullOrWhiteSpace(bannerUpdateDto.Title))
        {
            banner.Title = bannerUpdateDto.Title;
        }

        if (!string.IsNullOrWhiteSpace(bannerUpdateDto.SubTitle))
        {
            banner.SubTitle = bannerUpdateDto.SubTitle;
        }

        if (!string.IsNullOrWhiteSpace(bannerUpdateDto.Caption))
        {
            banner.Caption = bannerUpdateDto.Caption;
        }

        if (bannerUpdateDto.ScheduledStart.HasValue)
        {
            banner.ScheduledStart = bannerUpdateDto.ScheduledStart.Value;
        }

        if (bannerUpdateDto.ScheduledEnd.HasValue)
        {
            banner.ScheduledEnd = bannerUpdateDto.ScheduledEnd.Value;
        }

        _bannerRepo.Update(banner);
        await _bannerRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPut("{id:guid}/activate", Name = nameof(ActivateBanner))]
    public async Task<ActionResult> ActivateBanner(Guid id)
    {
        var banner = await _bannerRepo.GetByIdAsync(id);
        if (banner is null)
        {
            return NotFound();
        }

        banner.Current = true;
        _bannerRepo.Update(banner);
        await _bannerRepo.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpPut("{id:guid}/schedule", Name = nameof(ScheduleBanner))]
    public async Task<ActionResult> ScheduleBanner(Guid id, [FromBody] BannerScheduleUpdateDto bannerScheduleUpdateDto)
    {
        var banner = await _bannerRepo.GetByIdAsync(id);
        if (banner is null)
        {
            return NotFound();
        }

        banner.ScheduledStart = bannerScheduleUpdateDto.ScheduledStart;
        banner.ScheduledEnd = bannerScheduleUpdateDto.ScheduledEnd;

        _bannerRepo.Update(banner);
        await _bannerRepo.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}", Name = nameof(DeleteBanner))]
    public async Task<ActionResult> DeleteBanner(Guid id)
    {
        var banner = await _bannerRepo.GetByIdAsync(id);
        if (banner is null)
        {
            return NotFound();
        }

        _bannerRepo.HardDelete(banner);
        await _bannerRepo.SaveChangesAsync();
        return NoContent();
    }
}
