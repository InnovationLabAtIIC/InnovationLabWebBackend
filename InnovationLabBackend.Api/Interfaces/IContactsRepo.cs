using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Interfaces
{
    public interface IContactsRepo
    {
        Task<bool> SaveChangesAsync();
        Task<bool> PostContactAsync(Contact contact);
        Task<Contact> GetContactByIdAsync(Guid id);
        Task<List<Contact>> GetContactsAsync(int page, int limit);
    }
}
