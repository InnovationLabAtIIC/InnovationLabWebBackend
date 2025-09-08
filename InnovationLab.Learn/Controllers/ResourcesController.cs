using InnovationLab.Learn.DbContexts;
using InnovationLab.Learn.Dtos.Resources;
using InnovationLab.Learn.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLab.Learn.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ResourcesController(IRepository<LearnDbContext, Resource> repo) : ControllerBase
{
    private readonly IRepository<LearnDbContext, Resource> _repo = repo;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetResources))]
    public async Task<IActionResult> GetResources(int page = 1, int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var resources = await _repo.GetAsync(skip, pageSize);
        var resourcesDto = resources.Adapt<IList<ResourceReadDto>>();
        return Ok(resourcesDto);
    }

    [AllowAnonymous]
    [HttpPost(Name = nameof(CreateResource))]
    public async Task<IActionResult> CreateResource(ResourceCreateDto resourceCreateDto)
    {
        var resource = resourceCreateDto.Adapt<Resource>();
        await _repo.AddAsync(resource);
        await _repo.SaveChangesAsync();

        var resourceDto = resource.Adapt<ResourceReadDto>();
        // This shall be nameof(GetResourceById)
        return CreatedAtAction(nameof(GetResources), new { id = resourceDto.Id }, resourceDto);
    }
}