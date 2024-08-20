using Atlas.API.Extensions;
using Atlas.API.Interfaces;
using Atlas.API.Services;
using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Logging.Services;
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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<ISupportData, SupportData>();
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
    .AddPolicy(Auth.ATLAS_USER_CLAIM, policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole(Auth.ATLAS_USER_CLAIM);
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

bool generateSeedData = bool.Parse(builder.Configuration["SeedData:GenerateSeedData"] ?? "false");
bool generateSeedLogs = bool.Parse(builder.Configuration["SeedData:GenerateSeedLogs"] ?? "false");

if (generateSeedData)
{
    // Seed data for development testing purposes only...
    using IServiceScope scope = app.Services.CreateScope();

    IServiceProvider services = scope.ServiceProvider;

    ApplicationDbContext applicationDbContext = services.GetRequiredService<ApplicationDbContext>();

    applicationDbContext.SetUser("Atlas.SeedData");

    SeedData.Generate(applicationDbContext);

    if (generateSeedLogs)
    {
        ILogService logService = services.GetRequiredService<ILogService>();

        for (int i = 0; i < 500; i++)
        {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            logService.Log(Atlas.Core.Logging.Enums.LogLevel.Information, new AtlasException("myNumber is zero", new DivideByZeroException(), "myNumber=0"), "test@email.com");
            logService.Log(Atlas.Core.Logging.Enums.LogLevel.Warning, new AtlasException("myVariable is null", new NullReferenceException("myVariable"), "myVariable=null"), "system@email.com");
            logService.Log(Atlas.Core.Logging.Enums.LogLevel.Error, new AtlasException("Boom!", new StackOverflowException(), "what the...."), "user@email.com");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
        }
    }
}

app.Run();