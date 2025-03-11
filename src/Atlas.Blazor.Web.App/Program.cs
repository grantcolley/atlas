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

string? domain = null;
string? audience = null;
string? clientId = null;
string? clientSecret = null;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
if (builder.Environment.IsDevelopment())
{
    domain = builder.Configuration[Config.DEV_AUTH_DOMAIN] ?? throw new ArgumentNullException(Config.DEV_AUTH_DOMAIN);
    audience = builder.Configuration[Config.DEV_AUTH_AUDIENCE] ?? throw new ArgumentNullException(Config.DEV_AUTH_AUDIENCE);
    clientId = builder.Configuration[Config.DEV_AUTH_CLIENT_ID] ?? throw new ArgumentNullException(Config.DEV_AUTH_CLIENT_ID);
    clientSecret = builder.Configuration[Config.DEV_AUTH_CLIENT_SECRET] ?? throw new ArgumentNullException(Config.DEV_AUTH_CLIENT_SECRET);
}
else
{
    domain = builder.Configuration[Config.AZURE_AUTH_DOMAIN] ?? throw new ArgumentNullException(Config.AZURE_AUTH_DOMAIN);
    audience = builder.Configuration[Config.AZURE_AUTH_AUDIENCE] ?? throw new ArgumentNullException(Config.AZURE_AUTH_AUDIENCE);
    clientId = builder.Configuration[Config.AZURE_AUTH_CLIENT_ID] ?? throw new ArgumentNullException(Config.AZURE_AUTH_CLIENT_ID);
    clientSecret = builder.Configuration[Config.AZURE_AUTH_CLIENT_SECRET] ?? throw new ArgumentNullException(Config.AZURE_AUTH_CLIENT_SECRET);
}
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression

builder.Logging.ClearProviders();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
                  loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                        .Enrich.FromLogContext());

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

builder.Services.AddHttpClient(AtlasWeb.ATLAS_API,
      client => client.BaseAddress = new Uri(builder.Configuration[AtlasWeb.ATLAS_API] ?? throw new NullReferenceException(AtlasWeb.ATLAS_API)))
      .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient(AtlasWeb.ATLAS_API));

builder.Services.AddTransient<IClaimRequests, ClaimRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWeb.ATLAS_API);
    return new ClaimRequests(httpClient);
});

builder.Services.AddTransient<IDeveloperRequests, DeveloperRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWeb.ATLAS_API);
    return new DeveloperRequests(httpClient);
});

builder.Services.AddTransient<IGenericRequests, GenericRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWeb.ATLAS_API);
    return new GenericRequests(httpClient);
});

builder.Services.AddTransient<IOptionsRequests, OptionsRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWeb.ATLAS_API);
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
