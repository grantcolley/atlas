using Atlas.API.Interfaces;
using System.Security.Claims;

namespace Atlas.API.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Claim? _claim;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            ArgumentNullException.ThrowIfNull(nameof(httpContextAccessor));

            _httpContextAccessor = httpContextAccessor;

            if (_httpContextAccessor.HttpContext == null)
            {
                throw new NullReferenceException(nameof(_httpContextAccessor.HttpContext));
            }

            if (_httpContextAccessor.HttpContext.User.Identity == null)
            {
                throw new NullReferenceException(nameof(_httpContextAccessor.HttpContext.User.Identity));
            }

            _claim = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).FindFirst(ClaimTypes.Email);
        }

        public string? GetClaim()
        {
            return _claim?.Value;
        }
    }
}
