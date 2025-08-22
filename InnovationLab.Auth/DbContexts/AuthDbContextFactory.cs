using InnovationLab.Shared.Constants;
using InnovationLab.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InnovationLab.Auth.DbContexts;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        AuthDbContext context = DbContextFactoryHelper.CreateDbContext<AuthDbContext>(args);
        return context;
    }
}
