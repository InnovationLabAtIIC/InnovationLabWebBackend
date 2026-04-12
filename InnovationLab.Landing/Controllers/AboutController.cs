﻿using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InnovationLab.Landing.Dtos.About;
using InnovationLabBackend.Api.Dtos.About;
using InnovationLabBackend.Api.Interfaces;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AboutController : ControllerBase
    {
        private readonly IAboutRepo _aboutRepo;
        private readonly IMapper _mapper;
        private readonly ICloudinary _cloudinary;

        public AboutController(IAboutRepo aboutRepo, IMapper mapper, ICloudinary cloudinary)
        {
            _aboutRepo = aboutRepo;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        #region Public Endpoints

        [HttpGet(Name = "GetAbout")]
        public async Task<ActionResult<AboutResponseDto>> GetAbout()
        {
            var about = await _aboutRepo.GetAboutAsync();

            if (about == null)
            {
                about = new Models.About();
                await _aboutRepo.CreateOrUpdateAboutAsync(about);
            }

            var aboutDto = _mapper.Map<AboutResponseDto>(about);
            return Ok(aboutDto);
        }

        [HttpGet("mission-vision", Name = "GetMissionVision")]
        public async Task<ActionResult<AboutResponseDto>> GetMissionVision()
        {
            var about = await _aboutRepo.GetAboutAsync();

            if (about == null)
            {
                about = new Models.About();
                await _aboutRepo.CreateOrUpdateAboutAsync(about);
            }

            var aboutDto = _mapper.Map<AboutResponseDto>(about);
            return Ok(new { Mission = aboutDto.Mission, Vision = aboutDto.Vision });
        }

        [HttpGet("core-values", Name = "GetCoreValues")]
        public async Task<ActionResult<List<CoreValueResponseDto>>> GetCoreValues()
        {
            var coreValues = await _aboutRepo.GetCoreValuesAsync();
            var coreValuesDto = _mapper.Map<List<CoreValueResponseDto>>(coreValues);
            return Ok(coreValuesDto);
        }

        [HttpGet("parent-org", Name = "GetParentOrg")]
        public async Task<ActionResult<AboutResponseDto>> GetParentOrg()
        {
            var about = await _aboutRepo.GetAboutAsync();

            if (about == null)
            {
                about = new Models.About();
                await _aboutRepo.CreateOrUpdateAboutAsync(about);
            }

            var aboutDto = _mapper.Map<AboutResponseDto>(about);
            return Ok(new
            {
                ParentOrgName = aboutDto.ParentOrgName,
                ParentOrgDescription = aboutDto.ParentOrgDescription,
                ParentOrgLogoUrl = aboutDto.ParentOrgLogoUrl,
                ParentOrgWebsiteUrl = aboutDto.ParentOrgWebsiteUrl
            });
        }

        [HttpGet("journey", Name = "GetJourney")]
        public async Task<ActionResult<List<JourneyItemResponseDto>>> GetJourney()
        {
            var journeyItems = await _aboutRepo.GetJourneyItemsAsync();
            var journeyItemsDto = _mapper.Map<List<JourneyItemResponseDto>>(journeyItems);
            return Ok(journeyItemsDto);
        }

        #endregion

        #region Admin Endpoints

        [Authorize]
        [HttpPatch("mission-vision", Name = "UpdateMissionVision")]
        public async Task<ActionResult> UpdateMissionVision([FromBody] MissionVisionUpdateDto missionVisionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var about = await _aboutRepo.GetAboutAsync() ?? new Models.About();

            about.Mission = missionVisionDto.Mission;
            about.Vision = missionVisionDto.Vision;
            about.UpdatedAt = DateTimeOffset.UtcNow;

            await _aboutRepo.CreateOrUpdateAboutAsync(about);

            return NoContent();
        }

        [Authorize]
        [HttpPost("core-values", Name = "CreateCoreValue")]
        public async Task<ActionResult<CoreValueResponseDto>> CreateCoreValue([FromForm] CoreValueCreateDto coreValueDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coreValue = _mapper.Map<CoreValue>(coreValueDto);

            if (coreValueDto.Icon != null && coreValueDto.Icon.Length > 0)
            {
                var iconUrl = await UploadImage(coreValueDto.Icon, "core-values");
                if (iconUrl == null)
                {
                    return BadRequest("Icon upload failed");
                }
                coreValue.IconUrl = iconUrl;
            }

            var createdCoreValue = await _aboutRepo.CreateCoreValueAsync(coreValue);
            var responseDto = _mapper.Map<CoreValueResponseDto>(createdCoreValue);

            return CreatedAtAction(nameof(GetCoreValues), new { id = responseDto.Id }, responseDto);
        }

        [Authorize]
        [HttpPatch("core-values/{id}", Name = "UpdateCoreValue")]
        public async Task<ActionResult> UpdateCoreValue(Guid id, [FromForm] CoreValueUpdateDto coreValueDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coreValue = await _aboutRepo.GetCoreValueByIdAsync(id);

            if (coreValue == null)
            {
                return NotFound(new { Message = "Core value not found" });
            }

            if (coreValueDto.Title != null)
                coreValue.Title = coreValueDto.Title;

            if (coreValueDto.Description != null)
                coreValue.Description = coreValueDto.Description;

            if (coreValueDto.Order.HasValue)
                coreValue.Order = coreValueDto.Order.Value;

            if (coreValueDto.Icon != null && coreValueDto.Icon.Length > 0)
            {
                var iconUrl = await UploadImage(coreValueDto.Icon, "core-values");
                if (iconUrl == null)
                {
                    return BadRequest("Icon upload failed");
                }
                coreValue.IconUrl = iconUrl;
            }

            coreValue.UpdatedAt = DateTimeOffset.UtcNow;
            await _aboutRepo.UpdateCoreValueAsync(coreValue);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("core-values/{id}", Name = "DeleteCoreValue")]
        public async Task<ActionResult> DeleteCoreValue(Guid id)
        {
            var coreValue = await _aboutRepo.GetCoreValueByIdAsync(id);

            if (coreValue == null)
            {
                return NotFound(new { Message = "Core value not found" });
            }

            await _aboutRepo.DeleteCoreValueAsync(coreValue);

            return NoContent();
        }

        [Authorize]
        [HttpPatch("parent-org", Name = "UpdateParentOrg")]
        public async Task<ActionResult> UpdateParentOrg([FromForm] ParentOrgUpdateDto parentOrgDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var about = await _aboutRepo.GetAboutAsync() ?? new Models.About();

            about.ParentOrgName = parentOrgDto.ParentOrgName;
            about.ParentOrgDescription = parentOrgDto.ParentOrgDescription;
            about.ParentOrgWebsiteUrl = parentOrgDto.ParentOrgWebsiteUrl;

            if (parentOrgDto.ParentOrgLogo != null && parentOrgDto.ParentOrgLogo.Length > 0)
            {
                var logoUrl = await UploadImage(parentOrgDto.ParentOrgLogo, "parent-org");
                if (logoUrl == null)
                {
                    return BadRequest("Logo upload failed");
                }
                about.ParentOrgLogoUrl = logoUrl;
            }

            about.UpdatedAt = DateTimeOffset.UtcNow;
            await _aboutRepo.CreateOrUpdateAboutAsync(about);

            return NoContent();
        }

        [Authorize]
        [HttpPost("journey", Name = "CreateJourneyItem")]
        public async Task<ActionResult<JourneyItemResponseDto>> CreateJourneyItem([FromForm] JourneyItemCreateDto journeyItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var journeyItem = _mapper.Map<JourneyItem>(journeyItemDto);

            if (journeyItemDto.Image != null && journeyItemDto.Image.Length > 0)
            {
                var imageUrl = await UploadImage(journeyItemDto.Image, "journey");
                if (imageUrl == null)
                {
                    return BadRequest("Image upload failed");
                }
                journeyItem.ImageUrl = imageUrl;
            }

            var createdJourneyItem = await _aboutRepo.CreateJourneyItemAsync(journeyItem);
            var responseDto = _mapper.Map<JourneyItemResponseDto>(createdJourneyItem);

            return CreatedAtAction(nameof(GetJourney), new { id = responseDto.Id }, responseDto);
        }

        [Authorize]
        [HttpPatch("journey/{id}", Name = "UpdateJourneyItem")]
        public async Task<ActionResult> UpdateJourneyItem(Guid id, [FromForm] JourneyItemUpdateDto journeyItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var journeyItem = await _aboutRepo.GetJourneyItemByIdAsync(id);

            if (journeyItem == null)
            {
                return NotFound(new { Message = "Journey item not found" });
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
                var imageUrl = await UploadImage(journeyItemDto.Image, "journey");
                if (imageUrl == null)
                {
                    return BadRequest("Image upload failed");
                }
                journeyItem.ImageUrl = imageUrl;
            }

            journeyItem.UpdatedAt = DateTimeOffset.UtcNow;
            await _aboutRepo.UpdateJourneyItemAsync(journeyItem);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("journey/{id}", Name = "DeleteJourneyItem")]
        public async Task<ActionResult> DeleteJourneyItem(Guid id)
        {
            var journeyItem = await _aboutRepo.GetJourneyItemByIdAsync(id);

            if (journeyItem == null)
            {
                return NotFound(new { Message = "Journey item not found" });
            }

            await _aboutRepo.DeleteJourneyItemAsync(journeyItem);

            return NoContent();
        }

        #endregion

        #region Helper Methods

        private async Task<string?> UploadImage(IFormFile file, string folder)
        {
            if (file.Length <= 0) return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"innovation-lab/{folder}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        #endregion
    }
}
