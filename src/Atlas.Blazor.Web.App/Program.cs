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

string domainKey = Config.AUTH_DOMAIN;
string audienceKey = Config.AUTH_AUDIENCE;
string clientIdKey = Config.AUTH_CLIENT_ID;
string clientSecretKey = Config.AUTH_CLIENT_SECRET;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    domainKey = domainKey.Replace(":", "_");
    audienceKey = audienceKey.Replace(":", "_");
    clientIdKey = clientIdKey.Replace(":", "_");
    clientSecretKey = clientSecretKey.Replace(":", "_");
}

string? domain = builder.Configuration[domainKey] ?? throw new NullReferenceException(domainKey);
string? audience = builder.Configuration[audienceKey] ?? throw new NullReferenceException(audienceKey);
string? clientId = builder.Configuration[clientIdKey] ?? throw new NullReferenceException(clientIdKey);
string? clientSecret = builder.Configuration[clientSecretKey] ?? throw new NullReferenceException(clientSecretKey);

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

builder.Services.AddHttpClient(Config.ATLAS_API,
      client => client.BaseAddress = new Uri(builder.Configuration[Config.ATLAS_API] ?? throw new NullReferenceException(Config.ATLAS_API)))
      .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient(Config.ATLAS_API));

builder.Services.AddTransient<IClaimRequests, ClaimRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(Config.ATLAS_API);
    return new ClaimRequests(httpClient);
});

builder.Services.AddTransient<IDeveloperRequests, DeveloperRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(Config.ATLAS_API);
    return new DeveloperRequests(httpClient);
});

builder.Services.AddTransient<IGenericRequests, GenericRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(Config.ATLAS_API);
    return new GenericRequests(httpClient);
});

builder.Services.AddTransient<IOptionsRequests, OptionsRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(Config.ATLAS_API);
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
