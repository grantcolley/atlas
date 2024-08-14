![Alt text](/readme-images/Atlas.png?raw=true "Atlas") 

###### .NET 8.0, Blazor, ASP.NET Core Web API, Auth0, FluentUI, FluentValidation, Backend for Frontend (BFF), Entity Framework Core, MS SQL Server, SQLite

Atlas is a framework for hosting and building Blazor applications using the Backend for Frontend (BFF) pattern.

\
![Alt text](/readme-images/Atlas_Architecture.png?raw=true "Atlas Architecture")

[![Build status](https://ci.appveyor.com/api/projects/status/qx6pbauk9bfpopst?svg=true)](https://ci.appveyor.com/project/grantcolley/atlas)

## Table of Contents
* [Setup the Solution](#setup-the-solution)
    * [Multiple Startup Projects](#multiple-startup-projects)
    * [Atlas.API Configuration](#atlasapi-configuration)
    * [Atlas.Blazor.Web.App Configuration](#atlasblazorwebapp-configuration)
    * [Create the Database](#create-the-database)
    * [Insert Seed Data](#insert-seed-data)
* [Authentication](#authentication)
    * [Create an Auth0 Role](#create-an-auth0-role)
    * [Create Auth0 Users](#create-auth0-users)
    * [Securing Atlas.API](#securing-atlasapi)
    * [Securing Atlas.Blazor.Web.App](#securing-atlasblazorwebapp)
    * [Log In](#log-in)
* [Authorization](#authorization)
    * [Users, Roles and Permissions](#users-roles-and-permissions)
    * [Modules, Categories and Pages](#modules-categories-and-pages) 
* [Support](#support)
    * [Logging](#logging)
    * [Audit](#audit)
* [Notes](#notes)
    * [FluentDesignTheme Dark/Light](#fluentdesigntheme-darklight)
    * [Backend for frontend](#backend-for-frontend)

# Setup the Solution

### Multiple Startup Projects
In the _Solution Properties_, specify multiple startup projects and set the action for both **Atlas.API** Web API and **Atlas.Blazor.Web.App** Blazor application, to *Start*.

![Alt text](/readme-images/Solution_Property_Pages.png?raw=true "Solution Properties") 

### Atlas.API Configuration
In the **Atlas.API** [appsettings.json](https://github.com/grantcolley/atlas/blob/main/src/Atlas.API/appsettings.json) set the connection strings, configure [Auth0](https://auth0.com/) settings and generating seed data.

> [!NOTE]  
> Read the next section on [Authentication](#authentication) for how to configure [Auth0](https://auth0.com/) as the identity provider. 

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""   👈 set the Atlas database connection string
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
          "connectionString": "",   👈set the Atlas database connection string for Serilogs MS SqlServer
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
    "Domain": "",                        👈specify the Auth0 domain
    "Audience": "https://Atlas.API.com"  👈specify the audience
  },
  "SeedData": {
    "GenerateSeedData": "true", 👈 set to true to create seed data including modules, categories, pages, users, permissions and roles.
    "GenerateSeedLogs":  "true" 👈 set to true to generate mock logs
  }
}
```

### Atlas.Blazor.Web.App Configuration
In the **Atlas.Blazor.Web.App** [appsettings.json](https://github.com/grantcolley/atlas/blob/main/src/Atlas.Blazor.Web.App/appsettings.json) configure [Auth0](https://auth0.com/) settings and specify the **Atlas.API** url.

> [!NOTE]  
> Read the next section on [Authentication](#authentication) for how to configure [Auth0](https://auth0.com/) as the identity provider.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Auth0": {
    "Domain": "",                           👈specify the Auth0 domain
    "ClientId": "",                         👈specify the Auth0 ClientId
    "ClientSecret": "",                     👈specify the Auth0 ClientSecret
    "Audience": "https://Atlas.API.com"     👈specify the audience
  },
  "AtlasAPI": "https://localhost:44420"     👈specify the AtlasAPI url
}
```

### Create the Database
Use the `.NET CLI` for Entity Framework to create your database and create your schema from the migration. In the `Developer Powershell` or similar, navigate to the **Atlas.API** folder and run the following command.

```
dotnet ef database update --project ..\..\data\Atlas.Migrations.SQLServer
```

### Insert Seed Data
In the **Atlas.API** [appsettings.json](https://github.com/grantcolley/atlas/blob/f00c8113c0c79c44718d347a139ab877e63a7b88/src/Atlas.API/appsettings.json#L51-L53) configuration file set `GenerateSeedData` and `GenerateSeedLogs` to true. This will populate the database with seed data at startup.

> [!WARNING]  
> If `"GenerateSeedData": "true"` the tables in the Atlas database will be truncated and repopulated with seed data every time the application starts. Existing data will be permanently lost.

```json
  "SeedData": {
    "GenerateSeedData": "true", 👈 set to true to create seed data including modules, categories, pages, users, permissions and roles.
    "GenerateSeedLogs":  "true" 👈 set to true to generate mock logs
  }
```

# Authentication
Atlas is setup to use [Auth0](https://auth0.com/) as its authentication provider, although this can be swapped out for any provider supporting **OAuth 2.0**. With [Auth0](https://auth0.com/signup?place=header&type=button&text=sign%20up) you can create a free account and it has a easy to use dashboard for registering applications, and creating and managing roles and users.

Using the [Auth0](https://auth0.com/), register the **Atlas.API** Web API and **Atlas.Blazor.Web.App** Blazor application, and  create a `atlas-user` role and users.

### Create an Auth0 Role
In the [Auth0](https://auth0.com/) dashboard create a role called `atlas-user`. This role must be assigned to all users wishing to access the Atlas application.

> [!IMPORTANT]  
> Atlas users must be assigned the `atlas-user` role to access the Atlas application.

![Alt text](/readme-images/Auth0_Role.png?raw=true "Auth0 Role") 

### Create Auth0 Users
Create Auth0 users. The user's [Auth0](https://auth0.com/) email claim is mapped to the email of an authorised user in the Atlas database.

> [!IMPORTANT]  
> Atlas users must be assigned the `atlas-user` role to access the Atlas application.

![Alt text](/readme-images/Auth0_User.png?raw=true "Auth0 User") 

![Alt text](/readme-images/Auth0_User_Role.png?raw=true "Auth0 User Role") 

> [!TIP]
> [SeedData.cs](https://github.com/grantcolley/atlas/blob/9509c67c874711e1760bbf5cd6561c662abe2e81/data/Atlas.Seed.Data/SeedData.cs#L111-L114) already contains some pre-defined sample users with roles and permissions. Either create these users in [Auth0](https://auth0.com/), or amend the sample users in [SeedData.cs](https://github.com/grantcolley/atlas/blob/9509c67c874711e1760bbf5cd6561c662abe2e81/data/Atlas.Seed.Data/SeedData.cs#L111-L114) to reflect those created in [Auth0](https://auth0.com/).

> [!WARNING]  
> If `"GenerateSeedData": "true"` the tables in the Atlas database will be truncated and repopulated with seed data every time the application starts. Existing data will be permanently lost.

```C#
        private static void CreateUsers()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            users.Add("alice", new User { Name = "alice", Email = "alice@email.com" });
            users.Add("jane", new User { Name = "jane", Email = "jane@email.com" });
            users.Add("bob", new User { Name = "bob", Email = "bob@email.com" });
            users.Add("grant", new User { Name = "grant", Email = "grant@email.com" });

            foreach (User user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }
```



![Alt text](/readme-images/Auth0_User.png?raw=true "Auth0 Users") 

### Securing Atlas.API
The following article explains how to register and [secure a minimal WebAPI with Auth0](https://auth0.com/blog/securing-aspnet-minimal-webapis-with-auth0/) with the relevant parts in the **Atlas.API** [Program.cs](https://github.com/grantcolley/atlas/blob/main/src/Atlas.API/Program.cs).

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
            .RequireAuthorization(Auth.ATLAS_USER_CLAIM);  // 👈 add RequireAuthorization to endpoints

//....existing code removed for brevity

```

### Securing Atlas.Blazor.Web.App
The following article explains how to register and [add Auth0 Authentication to Blazor Web Apps](https://auth0.com/blog/auth0-authentication-blazor-web-apps/).

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

//....existing code removed for brevity

```

The following section in the article describes how the [client authentication state is synced with the server authentication state](https://auth0.com/blog/auth0-authentication-blazor-web-apps/#Syncing-the-Authentication-State) using [`PersistingRevalidatingAuthenticationStateProvider.cs`](https://github.com/grantcolley/atlas/blob/main/src/Atlas.Blazor.Web.App/Authentication/PersistingRevalidatingAuthenticationStateProvider.cs) in **Atlas.Blazor.Web.App** and [`PersistentAuthenticationStateProvider.cs`](https://github.com/grantcolley/atlas/blob/main/src/Atlas.Blazor.Web.App.Client/Authentication/PersistentAuthenticationStateProvider.cs) in **Atlas.Blazor.Web.App.Client**.

Finally, the following article describes how to [call protected APIs from a Blazor Web App](https://auth0.com/blog/call-protected-api-from-blazor-web-app/), including [calling external APIs](https://auth0.com/blog/call-protected-api-from-blazor-web-app/#Call-an-External-API) which requires injecting the access token into HTTP requests. This is handled by the [TokenHandler.cs](https://github.com/grantcolley/atlas/blob/main/src/Atlas.Blazor.Web.App/Authentication/TokenHandler.cs).

```C#
    public class TokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(_httpContextAccessor.HttpContext == null)  throw new NullReferenceException(nameof(_httpContextAccessor.HttpContext));

            string? accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
```

### Log In
Clicking the `Login` button on the top right corner of the application will re-direct the user to the [Auth0](https://auth0.com/) login page. Once authenticated, the user is directed back to the application, and the navigation panel will display the Modules, Categories and Pages the user has permission to access.

Login
![Alt text](/readme-images/Login.png?raw=true "Login") 

Authenticate
![Alt text](/readme-images/Auth0_Login.png?raw=true "Login") 

Callback redirects authenticated user back to **Atlas**.  
![Alt text](/readme-images/Logged_in.png?raw=true "Logged in") 

# Authorization
### Users, Roles and Permissions
Creating, updating and deleting users, roles and permissions, is done in the `Authorisation` category of the `Administration` module.

> [!TIP]
> The `Authorisation` category of the `Administration` module is only accessible to users who are members of the `Admin-Read Role` and `Admin-Write Role`.
> The `Admin-Read Role` gives read-only view of users, roles and permissions, while the `Admin-Write Role` permit creating, updating and deleting them.

![Alt text](/readme-images/Users.png?raw=true "Users") 

> [!IMPORTANT]  
> Every user must be assigned the `User` permission. This is done by making the user a member of the role `User Role`.

Here user Bob is assignd the roles `User Role` and `Support Role` roles.

![Alt text](/readme-images/User_Bob.png?raw=true "Bob the Support user") 

Here is a list of permissions created with seed data.

![Alt text](/readme-images/Permissions.png?raw=true "Permissions created with seed data") 

### Modules, Categories and Pages
Creating, updating and deleting modules, categories and pages, is done in the `Applications` category of the `Administration` module.

Modules are applications, and can be related or unrelated to each other. Each module consists of one or more categories. Each category groups related pages. A page is a routable `razor @page`.

> [!TIP]
> Because each page must point to a routable `razor @page`, the `Applications` category of the `Administration` module is only accessible to users who are members of the `Developer Role`.
> i.e. creating, updating and deleting modules, categories and pages is a developer concern.

Each module, category and page in the Navigation panel has a permission, and are only accessible to users who have been assigned that permission via role membership.

# Support
### Logging

### Audit

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


