![Alt text](/readme-images/Atlas.png?raw=true "Atlas") 

###### .NET 8.0, Blazor, ASP.NET Core Web API, Auth0, FluentUI, FluentValidation, Entity Framework Core, MS SQL Server, SQLite

\
![Alt text](/readme-images/Atlas_Architecture.png?raw=true "Atlas Architecture") 


[![Build status](https://ci.appveyor.com/api/projects/status/qx6pbauk9bfpopst?svg=true)](https://ci.appveyor.com/project/grantcolley/atlas)

## Table of Contents
* [Setup the Solution](#setup-the-solution)
* [Authentication](#authentication)
    * [Securing Atlas.API](#securing-atlasapi)
    * [Securing Atlas.Blazor.Web.App](#securing-atlasblazorwebapp)
* [Authorization](#authorization)
* [Support](#support)
    * [Logging](#logging)
* [Notes](#notes)
    * [FluentDesignTheme Dark/Light](#fluentdesigntheme-darklight)
    * [Backend for frontend](#backend-for-frontend)

# Setup the Solution

In the Solution Properties, specify multiple startup projects and set the action for both **Atlas.API** and **Atlas.Blazor.Web.App** to *Start*.

![Alt text](/readme-images/Solution_Property_Pages.png?raw=true "Solution Properties") 

In the **Atlas.API** [appsettings.json](https://github.com/grantcolley/atlas/blob/main/src/Atlas.API/appsettings.json) set the connection strings, configure Auth0 settings and generating seed data.

> [!NOTE]  
> Read the next section on [Authentication](#authentication) for how to configure Auth0 as the identity provider. 

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""   // 👈 set the default connection string
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.MSSqlServer" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Error",
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "MSSqlServer",
        "Args": {
          "connectionString": "",   // 👈set the Serilog MS SQL Server connection string
          "tableName": "Logs",
          "autoCreateSqlTable": true,
          "columnOptionsSection": {
            "customColumns": [
              {
                "ColumnName": "User",
                "DataType": "nvarchar",
                "DataLength": 450
              },
              {
                "ColumnName": "Context",
                "DataType": "nvarchar",
                "DataLength": 450
              }
            ]
          }
        }
      }
    ]
  },
  "AllowedHosts": "*",
  "Auth0": {
    "Domain": "",   // 👈specify the Auth0 domain
    "Audience": ""  // 👈specify the audience
  },
  "SeedData": {
    "GenerateSeedData": "true", // 👈 set to true
    "GenerateSeedLogs":  "true" // 👈 set to true
  }
}
```

# Authentication
Atlas uses [Auth0](https://auth0.com/) as its authentication provider. Create a free account with [Auth0](https://auth0.com/signup?place=header&type=button&text=sign%20up) and register the **Atlas.API** and **Atlas.Blazor.Web.App** in the Auth0 dashboard.

In the [Auth0](https://auth0.com/) dashboard, first create a role called `atlas-user`, and then create users and assign them the `atlas-user` role.

> [!TIP]
> [SeedData.cs](https://github.com/grantcolley/atlas/blob/9509c67c874711e1760bbf5cd6561c662abe2e81/data/Atlas.Seed.Data/SeedData.cs#L111-L114) already contains some pre-defined sample users with roles and permissions. Either create these users in [Auth0](https://auth0.com/), or amend the sample users in [SeedData.cs](https://github.com/grantcolley/atlas/blob/9509c67c874711e1760bbf5cd6561c662abe2e81/data/Atlas.Seed.Data/SeedData.cs#L111-L114) to reflect those created in [Auth0](https://auth0.com/).

![Alt text](/readme-images/Auth0_Role.png?raw=true "Auth0 Roles") 

![Alt text](/readme-images/Auth0_User.png?raw=true "Auth0 Users") 



## Securing Atlas.API
The following article explains how to [secure a minimal WebAPI with Auth0](https://auth0.com/blog/securing-aspnet-minimal-webapis-with-auth0/) with the relevant parts in the **Atlas.API** [Program.cs](https://github.com/grantcolley/atlas/blob/main/src/Atlas.API/Program.cs).

```C#

//....existing code removed for brevity

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Auth0:Domain"],
            ValidAudience = builder.Configuration["Auth0:Audience"]
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Auth.ATLAS_USER_CLAIM, policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole(Auth.ATLAS_USER_CLAIM);
    });

//....existing code removed for brevity

app.UseAuthentication();

app.UseAuthorization();

//....existing code removed for brevity

```

When mapping the minimal Web API methods add `RequireAuthorization(Auth.ATLAS_USER_CLAIM)`, as can be seen here in [AtlasEndpointMapper.cs](https://github.com/grantcolley/atlas/blob/main/src/Atlas.API/Extensions/AtlasEndpointMapper.cs).

```C#

//....existing code removed for brevity

app.MapGet($"/{AtlasAPIEndpoints.GET_CLAIM_MODULES}", ClaimEndpoint.GetClaimModules)
            .WithOpenApi()
            .WithName(AtlasAPIEndpoints.GET_CLAIM_MODULES)
            .WithDescription("Gets the user's authorized modules")
            .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(Auth.ATLAS_USER_CLAIM);  // 👈 add RequireAuthorization

//....existing code removed for brevity

```

## Securing Atlas.Blazor.Web.App
The following article explains how to [add Auth0 Authentication to Blazor Web Apps](https://auth0.com/blog/auth0-authentication-blazor-web-apps/).

Here are the relevant parts in the **Atlas.Blazor.Web.App** [Program.cs](https://github.com/grantcolley/atlas/blob/main/src/Atlas.Blazor.Web.App/Program.cs).

```C#

//....existing code removed for brevity

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

//....existing code removed for brevity

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

//....existing code removed for brevity

```

The following section in the article describes how the [client authentication state is synced with the server authentication state](https://auth0.com/blog/auth0-authentication-blazor-web-apps/#Syncing-the-Authentication-State) is with `PersistingRevalidatingAuthenticationStateProvider.cs`.

Finally, the following article describes how to [call protected APIs from a Blazor Web App](https://auth0.com/blog/call-protected-api-from-blazor-web-app/), including [calling external APIs](https://auth0.com/blog/call-protected-api-from-blazor-web-app/#Call-an-External-API) which requires injecting the access token into HTTP requests.

# Authorization

# Support
### Logging

# Notes
### FluentDesignTheme Dark/Light
What the [Fluent UI quick guide](https://fluentui-blazor.net/DesignTheme) doesn't tell you is you must also add a reference to `/_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css`.

For the Blazor Web App project, add the reference to the top of the `app.css` file in `wwwroot`:
```C#
@import '/_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css';
```

For the Blazor WebAssembly stand alone project, add the reference to the `index.html` file in `wwwroot`.
```C#
<Link href="_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
```

### Backend for frontend
- [Calling a Protected API from a Blazor Web App](https://auth0.com/blog/call-protected-api-from-blazor-web-app/)
- [Backend for Frontend (BFF)](https://datatracker.ietf.org/doc/html/draft-ietf-oauth-browser-based-apps#name-backend-for-frontend-bff)
- [Microsoft Architecture Patterns - Backends for Frontends](https://learn.microsoft.com/en-us/azure/architecture/patterns/backends-for-frontends)


