using InnovationLab.Landing.DbContexts;
using InnovationLab.Shared.Constants;
using InnovationLab.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDefaultConfiguration(builder.Environment);

builder.Services.AddOptionsConfigurations(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LandingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(ConfigurationKeys.PostgresConnection))
);

builder.Services.AddJwtAuth(builder.Configuration);

// Register Dependency Injections
builder.Services.AddSharedServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
