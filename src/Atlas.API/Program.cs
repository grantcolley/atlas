using Atlas.API.Extensions;
using Atlas.API.Interfaces;
using Atlas.API.Services;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Core.Validation.Extensions;
using Atlas.Data.Access.EF.Context;
using Atlas.Data.Access.EF.Data;
using Atlas.Data.Access.Interfaces;
using Atlas.Logging.Interfaces;
using Atlas.Logging.Serilog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Text.Json.Serialization;

string domainKey = Config.AUTH_DOMAIN;
string audienceKey = Config.AUTH_AUDIENCE;
string corsPolicyKey = Config.CORS_POLICY;
string originsUrls = Config.ORIGINS_URLS;
string createDatabaseKey = Config.DATABASE_CREATE;
string generateSeedDataKey = Config.DATABASE_SEED_DATA;
string generateSeedLogsKey = Config.DATABASE_SEED_LOGS;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    domainKey = domainKey.Replace(":", "_");
    audienceKey = audienceKey.Replace(":", "_");

    corsPolicyKey = corsPolicyKey.Replace(":", "_");
    originsUrls = originsUrls.Replace(":", "_");

    createDatabaseKey = createDatabaseKey.Replace(":", "_");
    generateSeedDataKey = generateSeedDataKey.Replace(":", "_");
    generateSeedLogsKey = generateSeedLogsKey.Replace(":", "_");
}

string? domain = builder.Configuration[domainKey] ?? throw new NullReferenceException(domainKey);
string? audience = builder.Configuration[audienceKey] ?? throw new NullReferenceException(audienceKey);
string? connectionString = builder.Configuration.GetConnectionString(Config.CONNECTION_STRING) ?? throw new NullReferenceException(Config.CONNECTION_STRING);
string? corsPolicy = builder.Configuration[corsPolicyKey] ?? throw new NullReferenceException(corsPolicyKey);
string? originUrls = builder.Configuration[originsUrls] ?? throw new NullReferenceException(originsUrls);

AtlasConfig atlasConfig = new()
{
    DatabaseCreate = bool.Parse(builder.Configuration[createDatabaseKey] ?? "false"),
    DatabaseSeedData = bool.Parse(builder.Configuration[generateSeedDataKey] ?? "false"),
    DatabaseSeedLogs = bool.Parse(builder.Configuration[generateSeedLogsKey] ?? "false")
};

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
              loggerConfiguration
              .MinimumLevel.Information()
              .Enrich.FromLogContext()
              .WriteTo.MSSqlServer(
                  connectionString: connectionString,
                  sinkOptions: new MSSqlServerSinkOptions
                  {
                      TableName = "Logs",
                      AutoCreateSqlDatabase = false
                  },
                  columnOptions: new ColumnOptions
                  {
                      AdditionalColumns =
                      [
                          new SqlColumn {ColumnName = "User", PropertyName = "User", DataType = SqlDbType.NVarChar, DataLength = 450},
                          new SqlColumn {ColumnName = "Context", PropertyName = "Context", DataType = SqlDbType.NVarChar, DataLength = 450},
                      ]
                  }));

builder.Services.AddAtlasValidators();

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
    if (connectionString.Contains(DataMigrations.SQLITE_DATABASE))
    {
        options.EnableSensitiveDataLogging()
                .UseSqlite(connectionString, x => x.MigrationsAssembly(DataMigrations.SQLITE_MIGRATIONS));
    }
    else
    {
        options.EnableSensitiveDataLogging()
                .UseSqlServer(connectionString, x => x.MigrationsAssembly(DataMigrations.SQLSERVER_MIGRATIONS));
    }
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(atlasConfig);
builder.Services.AddScoped<IClaimData, ClaimData>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IDeveloperData, DeveloperData>();
builder.Services.AddScoped<ISupportData, SupportData>();
builder.Services.AddScoped<IOptionsData, OptionsData>();
builder.Services.AddScoped<IApplicationData, ApplicationData>();
builder.Services.AddScoped<IAdministrationData, AdministrationData>();
builder.Services.AddScoped<IUserAuthorisationData, UserAuthorisationData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{domain}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = domain,
            ValidAudience = audience
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Auth.ATLAS_USER_CLAIM, policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole(Auth.ATLAS_USER_CLAIM);
    })
    .AddPolicy(Auth.ATLAS_DEVELOPER_CLAIM, policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole(Auth.ATLAS_DEVELOPER_CLAIM);
    });

if (!string.IsNullOrWhiteSpace(corsPolicy)
    && !string.IsNullOrWhiteSpace(originUrls))
{
    builder.Services.AddCors(options =>
    {
        string[] urls = originUrls.Split(';');

        options.AddPolicy(corsPolicy,
            builder =>
                builder.WithOrigins(urls)
                .AllowAnyHeader());
    });
}

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

if(!string.IsNullOrWhiteSpace(corsPolicy))
{ 
    app.UseCors(corsPolicy);
}

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();