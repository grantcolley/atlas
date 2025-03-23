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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

bool isDev = builder.Environment.IsDevelopment();

string? domain = builder.Configuration[Config.GET_AUTH_DOMAIN(isDev)] ?? throw new NullReferenceException(Config.GET_AUTH_DOMAIN(isDev));
string? audience = builder.Configuration[Config.GET_AUTH_AUDIENCE(isDev)] ?? throw new NullReferenceException(Config.GET_AUTH_AUDIENCE(isDev));
string? clientId = builder.Configuration[Config.GET_AUTH_CLIENT_ID(isDev)] ?? throw new NullReferenceException(Config.GET_AUTH_CLIENT_ID(isDev));
string? clientSecret = builder.Configuration[Config.GET_AUTH_CLIENT_SECRET(isDev)] ?? throw new NullReferenceException(Config.GET_AUTH_CLIENT_SECRET(isDev));

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

string atlasApi = Config.GET_ATLAS_API(isDev);

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
