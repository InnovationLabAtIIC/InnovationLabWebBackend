using InnovationLabBackend.Api.DbContext;
using InnovationLabBackend.Api.Interfaces;
using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InnovationLabBackend.Api.Repositories
{
    public class ContactsRepo(InnovationLabDbContext dbContext) : IContactsRepo
    {
        private readonly InnovationLabDbContext _dbContext = dbContext;

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> PostContactAsync(Contact contact)
        {
            await _dbContext.Contacts.AddAsync(contact);
            return await SaveChangesAsync();
        }

        public async Task<List<Contact>> GetContactsAsync(int page, int limit)
        {
            var contacts = await _dbContext.Contacts
                .Skip((page-1)*limit)
                .Take(limit)
                .ToListAsync();
            return contacts;
        }

        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            return await _dbContext.Contacts.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
