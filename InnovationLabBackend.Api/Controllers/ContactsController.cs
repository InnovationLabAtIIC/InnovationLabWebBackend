using AutoMapper;
using InnovationLabBackend.Api.Dtos.Contacts;
using InnovationLabBackend.Api.Interfaces;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnovationLabBackend.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepo _contactsRepo;
        private readonly IMapper _mapper;

        public ContactsController(IContactsRepo contactsRepo, IMapper mapper)
        {
            _contactsRepo = contactsRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> SubmitContactMessage([FromBody] PostContactDto postContactDto)
        {
            var contact = _mapper.Map<Contact>(postContactDto);
            var success = await _contactsRepo.PostContactAsync(contact);

            if (!success)
            {
                return StatusCode(500, "Failed to submit contact message.");
            }

            return Created(string.Empty, "Contact message submitted successfully");
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactResponseDto>>> GetContactMessage(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            var contacts = await _contactsRepo.GetContactsAsync(page, limit);
            var contactsResponseDto = _mapper.Map<List<ContactResponseDto>>(contacts);
            return Ok(contactsResponseDto);
        }
    }
}
