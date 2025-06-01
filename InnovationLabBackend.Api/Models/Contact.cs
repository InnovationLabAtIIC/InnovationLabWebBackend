using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Models
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email {get; set;}
        public required string PhoneNumber { get; set;}
        public required string Subject { get; set;}
        public required string Message { get; set;}
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    }
}
