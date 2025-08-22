using InnovationLab.Shared.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace InnovationLab.Landing.DbContexts;

public class LandingDbContextFactory : IDesignTimeDbContextFactory<LandingDbContext>
{
    public LandingDbContext CreateDbContext(string[] args)
    {
        LandingDbContext context = DbContextFactoryHelper.CreateDbContext<LandingDbContext>(args);
        return context;
    }
}
