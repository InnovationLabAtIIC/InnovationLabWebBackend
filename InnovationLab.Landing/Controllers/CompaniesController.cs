using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Companies;
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
public sealed class CompaniesController(
    IRepository<LandingDbContext, Company> companyRepo,
    IMediaService mediaService
) : ControllerBase
{
    private const string CompanyFolder = "companies";

    private readonly IRepository<LandingDbContext, Company> _companyRepo = companyRepo;
    private readonly IMediaService _mediaService = mediaService;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetCompanies))]
    public async Task<ActionResult<IList<CompanyResponseDto>>> GetCompanies(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;

        var companies = await _companyRepo.QueryAsync
        (
            query => query.OrderBy(c => c.Priority),
            skip,
            pageSize
        );

        var companyDtos = companies.Adapt<IList<CompanyResponseDto>>();
        return Ok(companyDtos);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetCompanyById))]
    public async Task<ActionResult<CompanyResponseDto>> GetCompanyById(Guid id)
    {
        var company = await _companyRepo.GetByIdAsync(id);
        if (company is null)
        {
            return NotFound();
        }

        return Ok(company.Adapt<CompanyResponseDto>());
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateCompany))]
    public async Task<ActionResult<CompanyResponseDto>> CreateCompany([FromForm] CompanyCreateDto companyCreateDto)
    {
        var mediaType = companyCreateDto.Logo.ContentType.ToMediaType();
        if (mediaType is not MediaType.Image)
        {
            return StatusCode(StatusCodes.Status415UnsupportedMediaType);
        }

        var logoUrl = await _mediaService.UploadAsync(
            file: companyCreateDto.Logo,
            mediaType: mediaType,
            folder: CompanyFolder
        );

        if (string.IsNullOrWhiteSpace(logoUrl))
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var company = companyCreateDto.Adapt<Company>();
        company.LogoUrl = logoUrl;

        await _companyRepo.AddAsync(company);
        await _companyRepo.SaveChangesAsync();

        var response = company.Adapt<CompanyResponseDto>();
        return CreatedAtAction(nameof(GetCompanyById), new { id = response.Id }, response);
    }

    [Authorize]
    [HttpPut("{id:guid}", Name = nameof(UpdateCompany))]
    public async Task<ActionResult> UpdateCompany(Guid id, [FromForm] CompanyUpdateDto companyUpdateDto)
    {
        var company = await _companyRepo.GetByIdAsync(id);
        if (company is null)
        {
            return NotFound();
        }

        var mediaType = companyUpdateDto.Logo.ContentType.ToMediaType();
        if (mediaType is not MediaType.Image)
        {
            return StatusCode(StatusCodes.Status415UnsupportedMediaType);
        }

        var logoUrl = await _mediaService.UploadAsync(
            file: companyUpdateDto.Logo,
            mediaType: mediaType,
            folder: CompanyFolder
        );

        if (string.IsNullOrWhiteSpace(logoUrl))
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        companyUpdateDto.Adapt(company);
        company.LogoUrl = logoUrl;

        _companyRepo.Update(company);
        await _companyRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}", Name = nameof(DeleteCompany))]
    public async Task<ActionResult> DeleteCompany(Guid id)
    {
        var company = await _companyRepo.GetByIdAsync(id);
        if (company is null)
        {
            return NotFound();
        }

        _companyRepo.SoftDelete(company);
        await _companyRepo.SaveChangesAsync();
        return NoContent();
    }
}
