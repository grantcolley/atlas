﻿using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Atlas.Blazor.Web.App.Authentication
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public TokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor == null) throw new NullReferenceException(nameof(_httpContextAccessor));
            if (_httpContextAccessor.HttpContext == null) throw new NullReferenceException(nameof(_httpContextAccessor.HttpContext));

            string? accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
