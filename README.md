![Alt text](/readme-images/Atlas.png?raw=true "Atlas") 

###### .NET 8.0, Blazor, ASP.NET Core Web API, Auth0, FluentUI, FluentValidation, Entity Framework Core, MS SQL Server, SQLite

\
![Alt text](/readme-images/Atlas_Architecture.png?raw=true "Atlas Architecture") 
\

[![Build status](https://ci.appveyor.com/api/projects/status/qx6pbauk9bfpopst?svg=true)](https://ci.appveyor.com/project/grantcolley/atlas)

## Table of Contents
* [Hosting Models](#hosting-models)
  * [Blazor WebAssembly](#blazor-webassembly)
  * [Blazor Server](#blazor-server)
* [Calling an External API](#calling-an-external-api) 
* [Authentication](#authentication)
  * [Blazor WebAssembly Standalone Authentication](#webassembly-standalone-authentication)
  * [Blazor Server Side Authentication](#server-side-authentication)
* [Logging](#logging)
* [Exception Handling](#exception-handling) 
* [Notes](#notes)
    * [FluentDesignTheme Dark/Light](#fluentdesigntheme-darklight) 

# Hosting Models
## Blazor WebAssembly
[Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-webassembly)

## Blazor Server
[Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-server)

# Calling an External API
[Scenarios for calling external API's](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0#server-side-scenarios-for-calling-external-web-apis)

[pass-tokens-to-a-server-side-blazor-app](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/additional-scenarios#pass-tokens-to-a-server-side-blazor-app)

# Authentication

[Identity vs OIDC Server](https://learn.microsoft.com/en-us/aspnet/core/security/how-to-choose-identity-solution?view=aspnetcore-8.0#identity-vs-oidc-server)

[Pass tokens to a server-side Blazor app](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/additional-scenarios?view=aspnetcore-8.0#pass-tokens-to-a-server-side-blazor-app)

## WebAssembly Standalone Authentication
[Secure an ASP.NET Core Blazor WebAssembly standalone app with the Authentication library](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library).

The Blazor WebAssembly Authentication library (`Authentication.js`) only supports the Proof Key for Code Exchange (PKCE) authorization code flow via the [Microsoft Authentication Library (MSAL, msal.js)](https://learn.microsoft.com/en-us/entra/identity-platform/msal-overview).

[Microsoft.AspNetCore.Components.WebAssembly.Authentication](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.Authentication) provides a set of primitives that help the app authenticate users and obtain tokens to call protected APIs.

[attach-tokens-to-outgoing-requests](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-8.0#attach-tokens-to-outgoing-requests)

[Additional Scenarios](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios)

## Server Side Authentication

[Calling a Protected API from a Blazor Web App](https://auth0.com/blog/call-protected-api-from-blazor-web-app/)

[Backend for Frontend (BFF)](https://datatracker.ietf.org/doc/html/draft-ietf-oauth-browser-based-apps#name-backend-for-frontend-bff)

[Microsoft Architecture Patterns - Backends for Frontends](https://learn.microsoft.com/en-us/azure/architecture/patterns/backends-for-frontends)

# Logging

# Exception Handling

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
