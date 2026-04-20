using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.EventAgendas;
using InnovationLab.Landing.Dtos.EventRegistrations;
using InnovationLab.Landing.Dtos.Events;
using InnovationLab.Landing.Enums;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EventsController(
    IRepository<LandingDbContext, Event> eventRepo,
    IRepository<LandingDbContext, EventAgenda> eventAgendaRepo,
    IRepository<LandingDbContext, EventRegistration> eventRegistrationRepo
) : ControllerBase
{
    private readonly IRepository<LandingDbContext, Event> _eventRepo = eventRepo;
    private readonly IRepository<LandingDbContext, EventAgenda> _eventAgendaRepo = eventAgendaRepo;
    private readonly IRepository<LandingDbContext, EventRegistration> _eventRegistrationRepo = eventRegistrationRepo;

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

        _eventRepo.SoftDelete(@event);
        await _eventRepo.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/agenda", Name = nameof(GetEventAgenda))]
    public async Task<ActionResult<IList<EventAgendaResponseDto>>> GetEventAgenda(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20
    )
    {
        var skip = (page - 1) * pageSize;

        var agendas = await _eventAgendaRepo.QueryAsync(
            ea => ea.Include(a => a.Items).Where(a => a.EventId == id),
            skip,
            pageSize
        );

        var agendasDto = agendas.Adapt<IList<EventAgendaResponseDto>>();

        return Ok(agendasDto);
    }

    [Authorize]
    [HttpPost("{id}/agenda", Name = nameof(CreateEventAgenda))]
    public async Task<ActionResult<EventAgendaResponseDto>> CreateEventAgenda(Guid id, [FromBody] EventAgendaCreateDto agendaCreateDto)
    {
        var @event = await _eventRepo.GetByIdAsync(id);

        if (@event is null)
        {
            return NotFound();
        }

        var newAgenda = agendaCreateDto.Adapt<EventAgenda>();
        newAgenda.EventId = id;

        await _eventAgendaRepo.AddAsync(newAgenda);
        await _eventAgendaRepo.SaveChangesAsync();

        var agendaDto = newAgenda.Adapt<EventAgendaResponseDto>();

        return CreatedAtAction(nameof(GetEventAgenda), new { id = agendaDto.Id }, agendaDto);
    }

    [Authorize]
    [HttpPut("/agenda/{agendaId}", Name = nameof(UpdateEventAgenda))]
    public async Task<ActionResult> UpdateEventAgenda(Guid agendaId, [FromBody] EventAgendaUpdateDto agendaUpdateDto)
    {
        var agenda = await _eventAgendaRepo.GetByIdAsync(agendaId);

        if (agenda is null)
        {
            return NotFound();
        }

        agendaUpdateDto.Adapt(agenda);
        _eventAgendaRepo.Update(agenda);
        await _eventAgendaRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("/agenda/{agendaId}", Name = nameof(DeleteEventAgenda))]
    public async Task<ActionResult> DeleteEventAgenda(Guid agendaId)
    {
        var agenda = await _eventAgendaRepo.GetByIdAsync(agendaId);

        if (agenda is null)
        {
            return NotFound();
        }

        _eventAgendaRepo.HardDelete(agenda);
        await _eventAgendaRepo.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/register", Name = nameof(RegisterForEvent))]
    public async Task<ActionResult<EventRegistrationResponseDto>> RegisterForEvent(Guid id, [FromBody] EventRegistrationCreateDto registrationCreateDto)
    {
        var @event = await _eventRepo.GetByIdAsync(id);

        if (@event is null)
        {
            return NotFound();
        }

        if (@event.IsTeamEvent)
        {
            if (registrationCreateDto.TeamMembers is null || registrationCreateDto.TeamMembers.Count is 0)
            {
                return BadRequest("Team Members are required for team event");
            }

            if (registrationCreateDto.TeamMembers.Count > @event.MaxTeamMembers)
            {
                return BadRequest($"Maximum of {@event.MaxTeamMembers} team members are only allowed");
            }
        }
        else
        {
            if (registrationCreateDto.TeamMembers is not null && registrationCreateDto.TeamMembers.Count is not 0)
            {
                return BadRequest("Team Members are not allowed for solo event");
            }
        }

        var newRegistration = registrationCreateDto.Adapt<EventRegistration>();
        newRegistration.EventId = id;
        newRegistration.Type = @event.IsTeamEvent ? EventRegistrationType.Team : EventRegistrationType.Solo;
        newRegistration.Status = EventRegistrationStatus.Pending;

        await _eventRegistrationRepo.AddAsync(newRegistration);
        await _eventRegistrationRepo.SaveChangesAsync();

        var registrationDto = newRegistration.Adapt<EventAgendaResponseDto>();

        return CreatedAtAction(nameof(GetEventRegistrations), new { id = registrationDto.Id }, registrationDto);
    }

    [Authorize]
    [HttpGet("{id}/registrations", Name = nameof(GetEventRegistrations))]
    public async Task<ActionResult<List<EventRegistrationResponseDto>>> GetEventRegistrations(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20
    )
    {
        var skip = (page - 1) * pageSize;

        var registrations = await _eventRegistrationRepo.QueryAsync(
            er => er.Where(r => r.EventId == id),
            skip,
            pageSize
        );

        var registrationsDto = registrations.Adapt<IList<EventRegistrationResponseDto>>();
        return Ok(registrationsDto);
    }

    [Authorize]
    [HttpPatch("/registrations/{registrationId}/status", Name = nameof(UpdateEventRegistrationStatus))]
    public async Task<ActionResult> UpdateEventRegistrationStatus(
        Guid registrationId,
        [FromBody] EventRegistrationUpdateDto registrationUpdateDto
    )
    {
        var registration = await _eventRegistrationRepo.GetByIdAsync(registrationId);

        if (registration is null)
        {
            return NotFound();
        }

        registrationUpdateDto.Adapt(registration);

        _eventRegistrationRepo.Update(registration);
        await _eventRegistrationRepo.SaveChangesAsync();

        return NoContent();
    }
}