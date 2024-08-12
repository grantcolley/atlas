![Alt text](/readme-images/Atlas.png?raw=true "Atlas") 

###### .NET 8.0, Blazor, ASP.NET Core Web API, Auth0, FluentUI, FluentValidation, Entity Framework Core, MS SQL Server, SQLite

\
![Alt text](/readme-images/Atlas_Architecture.png?raw=true "Atlas Architecture") 


[![Build status](https://ci.appveyor.com/api/projects/status/qx6pbauk9bfpopst?svg=true)](https://ci.appveyor.com/project/grantcolley/atlas)

## Table of Contents
* [Setup the Solution](#setup-the-solution)
* [Authentication](#authentication)
* [Authorization](#authorization)
* [Support](#support)
    * [Logging](#logging)
* [Notes](#notes)
    * [FluentDesignTheme Dark/Light](#fluentdesigntheme-darklight)
    * [Backend for frontend](#backend-for-frontend)

# Setup the Solution

# Authentication
Atlas uses [Auth0](https://auth0.com/) as its authentication provider. Create a free account with [Auth0](https://auth0.com/signup?place=header&type=button&text=sign%20up) and register the **Atlas.API** and **Atlas.Blazor.Web.App** in the Auth0 dashboard.

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
    .AddPolicy("atlas-user", policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole("atlas-user");
    });

//....existing code removed for brevity

app.UseAuthentication();

app.UseAuthorization();

//....existing code removed for brevity

```

The following articles explain how to [add Auth0 Authentication to Blazor Web Apps](https://auth0.com/blog/auth0-authentication-blazor-web-apps/) and to [Call Protected APIs from a Blazor Web App](https://auth0.com/blog/call-protected-api-from-blazor-web-app/).


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


