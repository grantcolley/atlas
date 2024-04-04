using Atlas.Blazor.WebAssembly;
using Atlas.Blazor.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    var identityProvider = builder.Configuration["IdentityProvider:DefaultProvider"];

    builder.Configuration.Bind(identityProvider, options.ProviderOptions);
    options.UserOptions.RoleClaim = "role";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration[$"{identityProvider}:Audience"]);
}).AddAccountClaimsPrincipalFactory<UserAccountFactory>();

builder.Services.AddFluentUIComponents();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, AtlasAuthenticationStateProvider>();
builder.Services.AddScoped<ITooltipService, TooltipService>();
// https://code-maze.com/authenticationstateprovider-blazor-webassembly/

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
