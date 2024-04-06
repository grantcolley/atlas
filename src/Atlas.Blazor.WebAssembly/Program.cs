using Atlas.Blazor.WebAssembly; 
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
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
});

builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
