using Atlas.Blazor.Web.App.Authentication;
using Atlas.Core.Authentication;
using Atlas.Core.Constants;
using Atlas.Requests.API;
using Atlas.Requests.Interfaces;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using System.IdentityModel.Tokens.Jwt;

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

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();
//builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<ITooltipService, TooltipService>();

builder.Services.AddHttpClient(AtlasConstants.ATLAS_API, client =>
{
    client.BaseAddress = new Uri("https://localhost:44420");
}).AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient(AtlasConstants.ATLAS_API));

builder.Services.AddTransient<IUserRequests, UserRequests>(sp =>
{
    //var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);

    //System.Diagnostics.Debug.WriteLine($"## AddTransient<IUserRequests, UserRequests> tokenProvider.AccessToken {tokenProvider.AccessToken}");

    return new UserRequests(httpClient);
});

builder.Services.AddTransient<IRequests, Requests>(sp =>
{
    //var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);

    //System.Diagnostics.Debug.WriteLine($"## AddTransient<IRequests, Requests> tokenProvider.AccessToken {tokenProvider.AccessToken?.Substring(0, 10)}");

    return new Requests(httpClient);
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

app.MapGet("login", async (HttpContext httpContext, string redirectUri = @"/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("logout", async (HttpContext httpContext, string redirectUri = @"/") =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapGet("/api/externalData", async (HttpClient httpClient) =>
{
    return await httpClient.GetFromJsonAsync<int[]>("data");
})
.RequireAuthorization();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(Atlas.Blazor.Components._Imports).Assembly)
    .AddInteractiveServerRenderMode();

app.Run();