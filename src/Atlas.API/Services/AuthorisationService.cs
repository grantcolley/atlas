using Atlas.API.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using System.Security.Claims;

namespace Atlas.API.Services
{
    public class AuthorisationService : IAuthorisationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorisationData _authorisationData;
        private readonly Claim? _claim;

        public AuthorisationService(IHttpContextAccessor httpContextAccessor, IAuthorisationData authorisationData)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new Exception(nameof(HttpContextAccessor));
            _authorisationData = authorisationData ?? throw new ArgumentNullException(nameof(authorisationData));

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

        public Task<Authorisation?> GetAuthorisationAsync()
        {
            return _authorisationData.GetAuthorisationAsync(_claim?.Value ?? string.Empty);
        }
    }
}
