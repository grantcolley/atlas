using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Helpers;
using Atlas.Blazor.Shared.Interfaces;
using Atlas.Blazor.Shared.Models;
using Atlas.Blazor.Shared.Services;
using Atlas.Core.Authentication;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.API;
using Atlas.Requests.Interfaces;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

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
    return new PageRouterService(PageRouterHelper.GetPageArgs());
});

builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IStateNotificationService, StateNotificationService>();
builder.Services.AddTransient<IDialogService, DialogService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
