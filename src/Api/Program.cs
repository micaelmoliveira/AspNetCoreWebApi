using Api.Configuration;
using Api.Extensions;
using Data.Context;
using Microsoft.EntityFrameworkCore;

// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ResolveDependencies();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.WebApiConfig();


// Configure the HTTP request pipeline.

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();

app.UseMvcConfiguration();

app.Run();
