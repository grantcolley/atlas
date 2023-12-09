using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Atlas.Blazor.Web.App.Components.Pages
{
    public class LoginModel : PageModel
    {
        public async Task OnGetAsync(string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.Content("~/");
            }

            if (HttpContext.User.Identity != null
                && HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(redirectUri)
                .Build();

            await HttpContext.ChallengeAsync(
                Auth0Constants.AuthenticationScheme,
                authenticationProperties).ConfigureAwait(false);
        }
    }
}