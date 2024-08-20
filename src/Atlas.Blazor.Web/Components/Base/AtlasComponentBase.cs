using Atlas.Blazor.Web.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Atlas.Blazor.Web.Components.Base
{
    public abstract class AtlasComponentBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState>? AuthenticationState { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ILogService? LoggingService { get; set; }

        protected string? _user { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationState != null)
            {
                AuthenticationState authenticationState = await AuthenticationState;
                _user = authenticationState.User?.Identity?.Name;
            }
        }

        protected T? GetResponse<T>(IResponse<T> response)
        {
            if (!response.IsSuccess
                && !string.IsNullOrWhiteSpace(response.Message))
            {
                RouteAlert(response.Message);
                return default;
            }
            else
            {
                return response.Result;
            }
        }

        protected void RouteAlert(string message, AtlasException? atlasException = null)
        {
            Models.Alert alert = new()
            {
                AlertType = Alerts.ERROR,
                Title = "Error",
                Message = message
            };

            LoggingService?.Log(Core.Logging.Enums.LogLevel.Error, message, atlasException, _user);

            NavigationManager?.NavigateTo(alert.Route);
        }
    }
}
