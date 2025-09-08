using InnovationLab.Shared.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace InnovationLab.Learn.DbContexts;

public class LearnDbContextFactory : IDesignTimeDbContextFactory<LearnDbContext>
{
    public LearnDbContext CreateDbContext(string[] args)
    {
        LearnDbContext context = DbContextFactoryHelper.CreateDbContext<LearnDbContext>(args);
        return context;
    }
}
