using InnovationLab.Landing.Models;
using InnovationLab.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Landing.DbContexts;

public class LandingDbContext(DbContextOptions<LandingDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(DatabaseSchemas.LandingSchema);
        base.OnModelCreating(builder);
    }
}