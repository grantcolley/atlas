using Atlas.Blazor.WebAssembly;
using Atlas.Blazor.WebAssembly.Authentication;
using Atlas.Core.Constants;
using Atlas.Requests.API;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddFluentUIComponents();

builder.Services
    .AddOidcAuthentication(options =>
    {
        // Configure your authentication provider options here.
        // For more information, see https://aka.ms/blazor-standalone-auth
        builder.Configuration.Bind("Auth0", options.ProviderOptions);
        options.UserOptions.RoleClaim = "role";
        options.ProviderOptions.ResponseType = "code";

        string? audience = builder.Configuration[$"Auth0:Audience"];

        if (!string.IsNullOrWhiteSpace(audience))
        {
            options.ProviderOptions.AdditionalProviderParameters.Add("audience", audience);
        }
    })
    .AddAccountClaimsPrincipalFactory<UserAccountFactory>();

builder.Services.AddCascadingAuthenticationState();

builder.Services
    .AddHttpClient(AtlasConstants.ATLAS_API, client =>
    {
        client.BaseAddress = new Uri("https://localhost:44420");
    })
    .AddHttpMessageHandler(sp =>
    {
        AuthorizationMessageHandler? handler = sp.GetService<AuthorizationMessageHandler>()?
        .ConfigureHandler(
            authorizedUrls: ["https://localhost:44420"],
            scopes: [Auth.ATLAS_USER_CLAIM]);

        if(handler == null) throw new NullReferenceException(nameof(handler));

        return handler;
    });

builder.Services.AddScoped<ITooltipService, TooltipService>();

builder.Services.AddTransient<IUserRequests, UserRequests>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(AtlasConstants.ATLAS_API);
    return new UserRequests(httpClient);
});

await builder.Build().RunAsync();
