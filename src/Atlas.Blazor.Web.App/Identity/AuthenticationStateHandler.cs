using Atlas.Core.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Atlas.Blazor.Web.App.Identity
{
    public class AuthenticationStateHandler : DelegatingHandler
    {
        readonly CircuitServicesAccessor? circuitServicesAccessor;

        public AuthenticationStateHandler(
            CircuitServicesAccessor circuitServicesAccessor)
        {
            this.circuitServicesAccessor = circuitServicesAccessor;

            TokenProvider? tokenProvider = circuitServicesAccessor?.Services?
                .GetRequiredService<TokenProvider>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TokenProvider? tokenProvider = circuitServicesAccessor?.Services?
                .GetRequiredService<TokenProvider>();

            if(tokenProvider != null) 
            {
                if (tokenProvider != null
                    && !string.IsNullOrWhiteSpace(tokenProvider.AccessToken)
                    && request.Headers.Authorization == null)
                {
                    request.Headers.Add("Authorization", $"Bearer {tokenProvider.AccessToken}");
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
