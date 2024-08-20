using Atlas.Blazor.Web.App.Authentication;
using Atlas.Blazor.Web.App.Components;
using Atlas.Blazor.Web.App.Extensions;
using Atlas.Blazor.Web.Constants;
using Atlas.Blazor.Web.Interfaces;
using Atlas.Blazor.Web.Services;
using Atlas.Core.Constants;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Logging.Services;
using Atlas.Requests.API;
using Atlas.Requests.Interfaces;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
                  loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                        .Enrich.FromLogContext());

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();

builder.Services
    .AddAuth0WebAppAuthentication(Auth0Constants.AuthenticationScheme, options =>
    {
        options.Domain = builder.Configuration["Auth0:Domain"] ?? throw new NullReferenceException("Auth0:Domain");
        options.ClientId = builder.Configuration["Auth0:ClientId"] ?? throw new NullReferenceException("Auth0:ClientId");
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"] ?? throw new NullReferenceException("Auth0:ClientSecret");
        options.ResponseType = "code";
    }).WithAccessToken(options =>
    {
        options.Audience = builder.Configuration["Auth0:Audience"] ?? throw new NullReferenceException("Auth0:Audience");
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddSingleton<IAtlasRoutesService, AtlasRoutesService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddScoped<IDialogService, DialogService>();
builder.Services.AddScoped<IAtlasDialogService, AtlasDialogService>();
builder.Services.AddScoped<IOptionsService, OptionsService>();

builder.Services.AddHttpClient(AtlasWebConstants.ATLAS_API,
      client => client.BaseAddress = new Uri(builder.Configuration[AtlasWebConstants.ATLAS_API] ?? throw new NullReferenceException(AtlasWebConstants.ATLAS_API)))
      .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient(AtlasWebConstants.ATLAS_API));

builder.Services.AddTransient<IClaimRequests, ClaimRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWebConstants.ATLAS_API);
    return new ClaimRequests(httpClient);
});

builder.Services.AddTransient<IGenericRequests, GenericRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWebConstants.ATLAS_API);
    return new GenericRequests(httpClient);
});

builder.Services.AddTransient<IOptionsRequests, OptionsRequests>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient(AtlasWebConstants.ATLAS_API);
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
            .AddAdditionalRoutableAssemblies(typeof(Atlas.Blazor.Web.App.Client._Imports).Assembly);

app.Run();
