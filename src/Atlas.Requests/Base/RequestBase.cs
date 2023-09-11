using Atlas.Core.Models;

namespace Atlas.Requests.Base
{
    public abstract class RequestBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly TokenProvider? _tokenProvider;

        protected RequestBase(HttpClient httpClient, TokenProvider tokenProvider)
            : this(httpClient)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
            }
        }

        protected RequestBase(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
    }
}
