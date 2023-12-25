using Atlas.Blazor.Web.App.Components;
using Atlas.Blazor.Web.App.Interfaces;
using Atlas.Blazor.Web.App.Routing;
using Atlas.Blazor.Web.App.Services;
using Atlas.Core.Authentication;
using Atlas.Core.Constants;
using Atlas.Requests.API;
using Atlas.Requests.Interfaces;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Weather.Client.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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

builder.Services.AddHttpClient(AtlasConstants.ATLAS_API, client =>
{
    client.BaseAddress = new Uri("https://localhost:44420");
});

builder.Services.AddSingleton<IPageRouterService, PageRouterService>(sp =>
{
    return new PageRouterService(PageRouterConfiguration.GetPageArgs());
});

builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IStateNotificationService, StateNotificationService>();
//builder.Services.AddTransient<IDialogService, DialogService>();

builder.Services.AddTransient<IUserRequests, UserRequests>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);
    return new UserRequests(httpClient, tokenProvider);
});

builder.Services.AddTransient<IOptionsRequest, OptionsRequest>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);
    return new OptionsRequest(httpClient, tokenProvider);
});

builder.Services.AddTransient<IGenericRequests, GenericRequests>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);
    return new GenericRequests(httpClient, tokenProvider);
});

builder.Services.AddTransient<IWeatherForecastRequests, WeatherForecastRequests>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);
    return new WeatherForecastRequests(httpClient, tokenProvider);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(Weather.Client._Imports).Assembly)
    .AddInteractiveServerRenderMode();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
