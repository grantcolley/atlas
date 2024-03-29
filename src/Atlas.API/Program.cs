using Atlas.API.Endpoints;
using Atlas.API.Extensions;
using Atlas.API.Interfaces;
using Atlas.API.Services;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Constants;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Data;
using Atlas.Data.Access.Interfaces;
using Atlas.Seed.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.IgnoreReadOnlyFields = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    bool? isSQLLite = builder.Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING)?.Contains(DataMigrations.SQLITE_DATABASE);

    if (isSQLLite.HasValue && isSQLLite.Value)
    {
        options.EnableSensitiveDataLogging()
                .UseSqlite(builder.Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING),
                            x => x.MigrationsAssembly(DataMigrations.SQLITE_MIGRATIONS));
    }
    else
    {
        options.EnableSensitiveDataLogging()
                .UseSqlServer(builder.Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING),
                            x => x.MigrationsAssembly(DataMigrations.SQLSERVER_MIGRATIONS));
    }
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IOptionsData, OptionsData>();
builder.Services.AddScoped<INavigationData, NavigationData>();
builder.Services.AddScoped<IAdministrationData, AdministrationData>();
builder.Services.AddScoped<IWeatherForecastData, WeatherForecastData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Auth0:Domain"],
            ValidAudience = builder.Configuration["Auth0:Audience"]
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("atlas-user", p => p.
        RequireAuthenticatedUser().
        RequireRole("atlas-user"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("local",
        builder =>
            builder.WithOrigins("https://localhost:44400", "https://localhost:44410")
                   .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.MapGet("/weatherforecast", WeatherForecastEndpoint.GetWeatherForecast)
    .WithOpenApi()
    .WithName("weatherorecast")
    .WithDescription("Gets the weather forecast")
    .Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError)
    .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

var useSeedData = bool.Parse(builder.Configuration["SeedData:UseSeedData"] ?? "false");

if (useSeedData)
{
    // Seed data for testing purposes only...
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();

    applicationDbContext.SetUser("Atlas.SeedData");

    SeedData.Initialise(applicationDbContext);
}

app.Run();