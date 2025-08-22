using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Events;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EventsController(IRepository<LandingDbContext, Event> repo) : ControllerBase
{
    private readonly IRepository<LandingDbContext, Event> _repo = repo;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetEvents))]
    public async Task<IActionResult> GetEvents(int page = 1, int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var events = await _repo.GetAsync(skip, pageSize);
        var eventsDto = events.Adapt<IList<EventReadDto>>();
        return Ok(eventsDto);
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = nameof(GetEventById))]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var ev = await _repo.GetByIdAsync(id);
        if (ev is null)
        {
            return NotFound();
        }

        var eventDto = ev.Adapt<EventReadDto>();
        return Ok(eventDto);
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateEvent))]
    public async Task<IActionResult> CreateEvent(EventCreateDto eventCreateDto)
    {
        var ev = eventCreateDto.Adapt<Event>();
        await _repo.AddAsync(ev);
        await _repo.SaveChangesAsync();

        var eventDto = ev.Adapt<EventReadDto>();
        return CreatedAtAction(nameof(GetEventById), new { id = eventDto.Id }, eventDto);
    }

    [Authorize]
    [HttpPut("{id}", Name = nameof(UpdateEvent))]
    public async Task<IActionResult> UpdateEvent(Guid id, EventUpdateDto eventUpdateDto)
    {
        var ev = await _repo.GetByIdAsync(id);
        if (ev is null)
        {
            return NotFound();
        }

        eventUpdateDto.Adapt(ev);
        _repo.Update(ev);
        await _repo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}", Name = nameof(DeleteEvent))]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var ev = await _repo.GetByIdAsync(id);
        if (ev is null)
        {
            return NotFound();
        }

        _repo.HardDelete(ev);
        await _repo.SaveChangesAsync();

        return NoContent();
    }
}