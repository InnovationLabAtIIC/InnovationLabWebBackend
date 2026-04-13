using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Dtos.Contacts;
using InnovationLab.Landing.Models;
using InnovationLab.Shared.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLab.Landing.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class ContactsController(IRepository<LandingDbContext, Contact> contactRepo) : ControllerBase
{
    private readonly IRepository<LandingDbContext, Contact> _contactRepo = contactRepo;

    [AllowAnonymous]
    [HttpPost(Name = nameof(CreateContactMessage))]
    public async Task<ActionResult<ContactResponseDto>> CreateContactMessage([FromBody] ContactCreateDto contactCreateDto)
    {
        var contact = contactCreateDto.Adapt<Contact>();
        await _contactRepo.AddAsync(contact);
        await _contactRepo.SaveChangesAsync();

        var dto = contact.Adapt<ContactResponseDto>();
        return CreatedAtAction(nameof(GetContactMessageById), new { id = dto.Id }, dto);
    }

    [Authorize]
    [HttpGet(Name = nameof(GetContactMessages))]
    public async Task<ActionResult<IList<ContactResponseDto>>> GetContactMessages([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        var contacts = await _contactRepo.GetAsync(skip, pageSize);
        var contactDtos = contacts.Adapt<IList<ContactResponseDto>>();
        return Ok(contactDtos);
    }

    [Authorize]
    [HttpGet("{id:guid}", Name = nameof(GetContactMessageById))]
    public async Task<ActionResult<ContactResponseDto>> GetContactMessageById(Guid id)
    {
        var contact = await _contactRepo.GetByIdAsync(id);
        if (contact is null)
        {
            return NotFound();
        }

        return Ok(contact.Adapt<ContactResponseDto>());
    }
}
