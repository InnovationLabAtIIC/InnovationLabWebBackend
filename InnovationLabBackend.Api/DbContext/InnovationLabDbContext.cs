using InnovationLabBackend.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InnovationLabBackend.Api.DbContext
{
    public class InnovationLabDbContext(DbContextOptions<InnovationLabDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<CoreValue> CoreValues { get; set; }
        public DbSet<JourneyItem> JourneyItems { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // Automatically convert all enums to strings
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.ClrType.GetProperties())
                {
                    if (property.PropertyType.IsEnum)
                    {
                        modelBuilder
                            .Entity(entity.ClrType)
                            .Property(property.Name)
                            .HasConversion<string>();
                    }

                    // Handle nullable enums
                    var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (underlyingType != null && underlyingType.IsEnum)
                    {
                        modelBuilder
                            .Entity(entity.ClrType)
                            .Property(property.Name)
                            .HasConversion(typeof(EnumToStringConverter<>).MakeGenericType(underlyingType));
                    }
                }
            }
        }
    }
}