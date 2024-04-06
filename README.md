![Alt text](/readme-images/Atlas.png?raw=true "Atlas") 

<!-- 
###### .NET 7.0, Blazor WebAssembly, Blazor Server, ASP.NET Core Web API, Auth0, MudBlazor, Entity Framework Core, MS SQL Server, SQLite 
######
-->

\
[![Build status](https://ci.appveyor.com/api/projects/status/qx6pbauk9bfpopst?svg=true)](https://ci.appveyor.com/project/grantcolley/atlas)
###### 

## Table of Contents
* [Hosting Models](#hosting-models)
  * [Blazor WebAssembly](#blazor-webassembly)
  * [Blazor Server](#blazor-server)
* [Authentication](#authentication)
  * [Blazor WebAssembly Standalone Authentication](#webassembly-standalone-authentication)
  * [Blazor Server Side Authentication](#server-side-authentication)

# Hosting Models
## Blazor WebAssembly
[Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-webassembly)

## Blazor Server
[Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-server)

# Authentication
## WebAssembly Standalone Authentication
[Secure an ASP.NET Core Blazor WebAssembly standalone app with the Authentication library](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library).

The Blazor WebAssembly Authentication library (`Authentication.js`) only supports the Proof Key for Code Exchange (PKCE) authorization code flow via the [Microsoft Authentication Library (MSAL, msal.js)](https://learn.microsoft.com/en-us/entra/identity-platform/msal-overview).

[Microsoft.AspNetCore.Components.WebAssembly.Authentication](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.Authentication) provides a set of primitives that help the app authenticate users and obtain tokens to call protected APIs.

## Server Side Authentication

 
  
