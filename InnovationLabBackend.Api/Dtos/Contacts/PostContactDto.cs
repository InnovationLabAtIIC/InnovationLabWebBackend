using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.Contacts
{
    [AutoMap(typeof(Contact), ReverseMap = true)]
    public class PostContactDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Subject { get; set; }
        public required string Message { get; set; }
    }
}
