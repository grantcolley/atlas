using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Atlas.Core.Models;

namespace Atlas.Blazor.Web.App.Client.Authentication
{
    public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> _unauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!persistentState.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
            {
                return _unauthenticatedTask;
            }

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)];

            return Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(PersistentAuthenticationStateProvider)))));
        }
    }
}
