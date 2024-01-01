using Atlas.Core.Authentication;

namespace Atlas.Requests.Interfaces
{
    public interface IRequestBase
    {
        void SetBearerToken(TokenProvider? tokenProvider);
    }
}
