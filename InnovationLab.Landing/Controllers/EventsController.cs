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
public sealed class EventsController(IRepository<LandingDbContext, Event> eventRepo) : ControllerBase
{
    private readonly IRepository<LandingDbContext, Event> _eventRepo = eventRepo;

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetEvents))]
    public async Task<ActionResult<IList<EventResponseDto>>> GetEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var events = await _eventRepo.GetAsync(skip, pageSize);
        var eventsDto = events.Adapt<IList<EventResponseDto>>();
        return Ok(eventsDto);
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = nameof(GetEventById))]
    public async Task<ActionResult<EventResponseDto>> GetEventById(Guid id)
    {
        var @event = await _eventRepo.GetByIdAsync(id);
        if (@event is null)
        {
            return NotFound();
        }

        var eventDto = @event.Adapt<EventResponseDto>();
        return Ok(eventDto);
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateEvent))]
    public async Task<ActionResult<EventResponseDto>> CreateEvent([FromForm] EventCreateDto eventCreateDto)
    {
        var @event = eventCreateDto.Adapt<Event>();
        await _eventRepo.AddAsync(@event);
        await _eventRepo.SaveChangesAsync();

        var eventDto = @event.Adapt<EventResponseDto>();
        return CreatedAtAction(nameof(GetEventById), new { id = eventDto.Id }, eventDto);
    }

    [Authorize]
    [HttpPut("{id}", Name = nameof(UpdateEvent))]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromForm] EventUpdateDto eventUpdateDto)
    {
        var @event = await _eventRepo.GetByIdAsync(id);
        if (@event is null)
        {
            return NotFound();
        }

        eventUpdateDto.Adapt(@event);
        _eventRepo.Update(@event);
        await _eventRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}", Name = nameof(DeleteEvent))]
    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        var @event = await _eventRepo.GetByIdAsync(id);
        if (@event is null)
        {
            return NotFound();
        }

        _eventRepo.HardDelete(@event);
        await _eventRepo.SaveChangesAsync();

        return NoContent();
    }
}