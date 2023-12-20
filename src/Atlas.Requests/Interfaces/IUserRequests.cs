﻿using Atlas.Core.Models;
using System.Security.Claims;

namespace Atlas.Requests.Interfaces
{
    public interface IUserRequests
    {
        Task<string?> GetThemeAsync();
        Task SetThemeAsync(string theme);
        Task<IEnumerable<Module>?> GetClaimModulesAsync();
        Task<Authorisation?> GetAuthorisationAsync(ClaimsPrincipal principal);
    }
}
