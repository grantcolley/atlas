using Atlas.API.Extensions;
using Atlas.API.Interfaces;
using Atlas.API.Services;
using Atlas.Data.Access.Constants;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Data;
using Atlas.Data.Access.Interfaces;
using Atlas.Seed.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
                  loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                        .Enrich.FromLogContext());

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Atlas.API", Version = "v1" });
    });

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

builder.Services.AddScoped<IClaimData, ClaimData>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IOptionsData, OptionsData>();
builder.Services.AddScoped<IApplicationData, ApplicationData>();
builder.Services.AddScoped<IAdministrationData, AdministrationData>();

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("atlas-user", policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole("atlas-user");
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("local",
        builder =>
            builder.WithOrigins("https://localhost:44400", "https://localhost:44410")
            .AllowAnyHeader());
});

WebApplication app = builder.Build();

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

app.UseCors("local");

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

bool useSeedData = bool.Parse(builder.Configuration["SeedData:UseSeedData"] ?? "false");

if (useSeedData)
{
    // Seed data for development testing purposes only...
    using IServiceScope scope = app.Services.CreateScope();

    IServiceProvider services = scope.ServiceProvider;

    ApplicationDbContext applicationDbContext = services.GetRequiredService<ApplicationDbContext>();

    applicationDbContext.SetUser("Atlas.SeedData");

    SeedData.Initialise(applicationDbContext);
}

app.Run();