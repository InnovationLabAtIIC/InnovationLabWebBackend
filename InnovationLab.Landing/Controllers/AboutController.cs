﻿using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.About;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Enums;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class AboutController(
    IRepository<LandingDbContext, About> aboutRepo,
    IRepository<LandingDbContext, CoreValue> coreValueRepo,
    IRepository<LandingDbContext, JourneyItem> journeyItemRepo,
    IMediaService mediaService
) : ControllerBase
{
    private const string CoreValuesFolder = "core-values";
    private const string ParentOrgFolder = "parent-org";
    private const string JourneyFolder = "journey";

    private readonly IRepository<LandingDbContext, About> _aboutRepo = aboutRepo;
    private readonly IRepository<LandingDbContext, CoreValue> _coreValueRepo = coreValueRepo;
    private readonly IRepository<LandingDbContext, JourneyItem> _journeyItemRepo = journeyItemRepo;
    private readonly IMediaService _mediaService = mediaService;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetAbout))]
    public async Task<ActionResult<AboutResponseDto>> GetAbout()
    {
        var about = (await _aboutRepo.GetAsync(0, 1)).FirstOrDefault();
        if (about is null)
        {
            return NotFound();
        }

        return Ok(about.Adapt<AboutResponseDto>());
    }

    [AllowAnonymous]
    [HttpGet("core-values", Name = nameof(GetCoreValues))]
    public async Task<ActionResult<IList<CoreValueResponseDto>>> GetCoreValues([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var coreValues = await _coreValueRepo.QueryAsync(q => q.OrderBy(c => c.Order), skip, pageSize);
        var coreValuesDto = coreValues.Adapt<IList<CoreValueResponseDto>>();
        return Ok(coreValuesDto);
    }

    [AllowAnonymous]
    [HttpGet("journey", Name = nameof(GetJourney))]
    public async Task<ActionResult<IList<JourneyItemResponseDto>>> GetJourney([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var journeyItems = await _journeyItemRepo.QueryAsync(q => q.OrderBy(j => j.Order).ThenBy(j => j.Date), skip, pageSize);
        var journeyItemsDto = journeyItems.Adapt<IList<JourneyItemResponseDto>>();
        return Ok(journeyItemsDto);
    }

    [Authorize]
    [HttpPost(Name = nameof(UpsertAbout))]
    public async Task<ActionResult<AboutResponseDto>> UpsertAbout([FromForm] AboutUpsertDto aboutUpsertDto)
    {
        var about = (await _aboutRepo.GetAsync(0, 1)).FirstOrDefault();
        var isCreated = false;

        if (about is null)
        {
            about = new About();
            isCreated = true;
        }

        about.Mission = aboutUpsertDto.Mission;
        about.Vision = aboutUpsertDto.Vision;
        about.ParentOrgName = aboutUpsertDto.ParentOrgName;
        about.ParentOrgDescription = aboutUpsertDto.ParentOrgDescription;
        about.ParentOrgWebsiteUrl = aboutUpsertDto.ParentOrgWebsiteUrl;

        if (aboutUpsertDto.ParentOrgLogo is not null && aboutUpsertDto.ParentOrgLogo.Length > 0)
        {
            var logoUrl = await _mediaService.UploadAsync(aboutUpsertDto.ParentOrgLogo, MediaType.Image, ParentOrgFolder);
            if (string.IsNullOrWhiteSpace(logoUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            about.ParentOrgLogoUrl = logoUrl;
        }

        if (isCreated)
        {
            await _aboutRepo.AddAsync(about);
        }
        else
        {
            _aboutRepo.Update(about);
        }

        await _aboutRepo.SaveChangesAsync();

        var responseDto = about.Adapt<AboutResponseDto>();

        if (isCreated)
        {
            return CreatedAtAction(nameof(GetAbout), new { id = responseDto.Id }, responseDto);
        }

        return Ok(responseDto);
    }

    [Authorize]
    [HttpPost("core-values", Name = nameof(CreateCoreValue))]
    public async Task<ActionResult<CoreValueResponseDto>> CreateCoreValue([FromForm] CoreValueCreateDto coreValueDto)
    {
        var coreValue = coreValueDto.Adapt<CoreValue>();

        if (coreValueDto.Icon != null && coreValueDto.Icon.Length > 0)
        {
            var iconUrl = await _mediaService.UploadAsync(coreValueDto.Icon, InnovationLab.Shared.Enums.MediaType.Image, CoreValuesFolder);
            if (string.IsNullOrWhiteSpace(iconUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            coreValue.IconUrl = iconUrl;
        }

        await _coreValueRepo.AddAsync(coreValue);
        await _coreValueRepo.SaveChangesAsync();

        var responseDto = coreValue.Adapt<CoreValueResponseDto>();

        return CreatedAtAction(nameof(GetCoreValues), new { id = responseDto.Id }, responseDto);
    }

    [Authorize]
    [HttpPatch("core-values/{id:guid}", Name = nameof(UpdateCoreValue))]
    public async Task<ActionResult> UpdateCoreValue(Guid id, [FromForm] CoreValueUpdateDto coreValueDto)
    {
        var coreValue = await _coreValueRepo.GetByIdAsync(id);
        if (coreValue is null)
        {
            return NotFound();
        }

        if (coreValueDto.Title != null)
            coreValue.Title = coreValueDto.Title;

        if (coreValueDto.Description != null)
            coreValue.Description = coreValueDto.Description;

        if (coreValueDto.Order.HasValue)
            coreValue.Order = coreValueDto.Order.Value;

        if (coreValueDto.Icon != null && coreValueDto.Icon.Length > 0)
        {
            var iconUrl = await _mediaService.UploadAsync(coreValueDto.Icon, InnovationLab.Shared.Enums.MediaType.Image, CoreValuesFolder);
            if (string.IsNullOrWhiteSpace(iconUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            coreValue.IconUrl = iconUrl;
        }

        _coreValueRepo.Update(coreValue);
        await _coreValueRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("core-values/{id:guid}", Name = nameof(DeleteCoreValue))]
    public async Task<ActionResult> DeleteCoreValue(Guid id)
    {
        var coreValue = await _coreValueRepo.GetByIdAsync(id);
        if (coreValue is null)
        {
            return NotFound();
        }

        _coreValueRepo.HardDelete(coreValue);
        await _coreValueRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPost("journey", Name = nameof(CreateJourneyItem))]
    public async Task<ActionResult<JourneyItemResponseDto>> CreateJourneyItem([FromForm] JourneyItemCreateDto journeyItemDto)
    {
        var journeyItem = journeyItemDto.Adapt<JourneyItem>();

        if (journeyItemDto.Image != null && journeyItemDto.Image.Length > 0)
        {
            var imageUrl = await _mediaService.UploadAsync(journeyItemDto.Image, InnovationLab.Shared.Enums.MediaType.Image, JourneyFolder);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            journeyItem.ImageUrl = imageUrl;
        }

        await _journeyItemRepo.AddAsync(journeyItem);
        await _journeyItemRepo.SaveChangesAsync();

        var responseDto = journeyItem.Adapt<JourneyItemResponseDto>();

        return CreatedAtAction(nameof(GetJourney), new { id = responseDto.Id }, responseDto);
    }

    [Authorize]
    [HttpPatch("journey/{id:guid}", Name = nameof(UpdateJourneyItem))]
    public async Task<ActionResult> UpdateJourneyItem(Guid id, [FromForm] JourneyItemUpdateDto journeyItemDto)
    {
        var journeyItem = await _journeyItemRepo.GetByIdAsync(id);
        if (journeyItem is null)
        {
            return NotFound();
        }

        if (journeyItemDto.Title != null)
            journeyItem.Title = journeyItemDto.Title;

        if (journeyItemDto.Description != null)
            journeyItem.Description = journeyItemDto.Description;

        if (journeyItemDto.Date.HasValue)
            journeyItem.Date = journeyItemDto.Date.Value;

        if (journeyItemDto.Order.HasValue)
            journeyItem.Order = journeyItemDto.Order.Value;

        if (journeyItemDto.Image != null && journeyItemDto.Image.Length > 0)
        {
            var imageUrl = await _mediaService.UploadAsync(journeyItemDto.Image, InnovationLab.Shared.Enums.MediaType.Image, JourneyFolder);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            journeyItem.ImageUrl = imageUrl;
        }

        _journeyItemRepo.Update(journeyItem);
        await _journeyItemRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("journey/{id:guid}", Name = nameof(DeleteJourneyItem))]
    public async Task<ActionResult> DeleteJourneyItem(Guid id)
    {
        var journeyItem = await _journeyItemRepo.GetByIdAsync(id);
        if (journeyItem is null)
        {
            return NotFound();
        }

        _journeyItemRepo.HardDelete(journeyItem);
        await _journeyItemRepo.SaveChangesAsync();

        return NoContent();
    }

}
