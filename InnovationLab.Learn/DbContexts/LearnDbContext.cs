using InnovationLab.Learn.Models;
using InnovationLab.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Learn.DbContexts;

public class LearnDbContext(DbContextOptions<LearnDbContext> options) : DbContext(options)
{
    public DbSet<Resource> Resources { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(DatabaseSchemas.LearnSchema);
        base.OnModelCreating(builder);
    }
}