using Atlas.Blazor.Web.App.Authentication;
using Atlas.Blazor.Web.App.Components;
using Atlas.Blazor.Web.App.Extensions;
using Atlas.Blazor.Web.Interfaces;
using Atlas.Blazor.Web.Services;
using Atlas.Core.Constants;
using Atlas.Core.Validation.Extensions;
using Atlas.Logging.Interfaces;
using Atlas.Logging.Serilog.Services;
using Atlas.Requests.API;
using Atlas.Requests.Interfaces;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

string? atlasApi = Config.ATLAS_API;
string? connectionString = builder.Configuration.GetConnectionString(Config.CONNECTION_STRING) ?? throw new NullReferenceException(Config.CONNECTION_STRING);
string? domain = builder.Configuration[Config.AUTH_DOMAIN] ?? throw new NullReferenceException(Config.AUTH_DOMAIN);
string? audience = builder.Configuration[Config.AUTH_AUDIENCE] ?? throw new NullReferenceException(Config.AUTH_AUDIENCE);
string? clientId = builder.Configuration[Config.AUTH_CLIENT_ID] ?? throw new NullReferenceException(Config.AUTH_CLIENT_ID);
string? clientSecret = builder.Configuration[Config.AUTH_CLIENT_SECRET] ?? throw new NullReferenceException(Config.AUTH_CLIENT_SECRET);

builder.Logging.ClearProviders();

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

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddFluentUIComponents(new LibraryConfiguration { UseTooltipServiceProvider = true });

builder.Services.AddAtlasValidators();

builder.Services
    .AddAuth0WebAppAuthentication(Auth0Constants.AuthenticationScheme, options =>
    {
        options.Domain = domain;
        options.ClientId = clientId;
        options.ClientSecret = clientSecret;
        options.ResponseType = "code";
    }).WithAccessToken(options =>
    {
        options.Audience = audience;
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();

builder.Services.AddSingleton<IAtlasRoutesService, AtlasRoutesService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IAtlasDialogService, AtlasDialogService>();
builder.Services.AddScoped<IOptionsService, OptionsService>();

builder.Services.AddHttpClient(atlasApi,
      client => client.BaseAddress = new Uri(builder.Configuration[atlasApi] ?? throw new NullReferenceException(atlasApi)))
      .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient(atlasApi));

builder.Services.AddTransient<IClaimRequests, ClaimRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(atlasApi);
    return new ClaimRequests(httpClient);
});

builder.Services.AddTransient<IDeveloperRequests, DeveloperRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(atlasApi);
    return new DeveloperRequests(httpClient);
});

builder.Services.AddTransient<IGenericRequests, GenericRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(atlasApi);
    return new GenericRequests(httpClient);
});

builder.Services.AddTransient<IOptionsRequests, OptionsRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(atlasApi);
    return new OptionsRequests(httpClient);
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("login", async (HttpContext httpContext, string redirectUri = @"/") =>
{
    AuthenticationProperties authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("logout", async (HttpContext httpContext, string redirectUri = @"/") =>
{
    AuthenticationProperties authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapGet($"/{AtlasAPIEndpoints.GET_CLAIM_MODULES}", async (HttpClient httpClient) =>
{
    return await httpClient.GetFromJsonAsync<int[]>(AtlasAPIEndpoints.GET_CLAIM_MODULES);
})
.RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(Atlas.Blazor.Web.App.Client._Imports).Assembly,
        typeof(Atlas.Blazor.Web._Imports).Assembly);

app.Services.AddAtlasRoutablePages()
            .AddAdditionalRoutableAssemblies(
                typeof(Atlas.Blazor.Web.App.Client._Imports).Assembly);

app.Run();
