using Atlas.Core.Models;

namespace Atlas.Requests.Base
{
    public abstract class RequestBase
    {
        protected readonly HttpClient httpClient;
        protected readonly TokenProvider? tokenProvider;

        protected RequestBase(HttpClient httpClient, TokenProvider tokenProvider)
            : this(httpClient)
        {
            this.tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

            if (httpClient.DefaultRequestHeaders.Authorization == null)
            {
                var token = tokenProvider.AccessToken;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        protected RequestBase(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
    }
}
