using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using System.Security.Claims;

namespace Atlas.API.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Claim? _claim;
        private readonly Claim? _developerClaim;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
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

            IEnumerable<Claim> roles = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).FindAll(ClaimTypes.Role);

            foreach(Claim claim in roles)
            {
                if(claim.Value == Auth.ATLAS_DEVELOPER_CLAIM)
                {
                    _developerClaim = claim;
                    break;
                }
            }
        }

        public string? GetClaim()
        {
            return _claim?.Value;
        }

        public bool HasDeveloperClaim()
        {
            return _developerClaim != null;
        }
    }
}
