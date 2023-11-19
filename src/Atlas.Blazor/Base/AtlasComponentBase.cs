using Atlas.Blazor.Constants;
using Atlas.Blazor.Interfaces;
using Atlas.Blazor.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Base
{
    public abstract class AtlasComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IStateNotificationService? StateNotificationService { get; set; }

        [Inject]
        public IDialogService? DialogService { get; set; }

        protected T? GetResponse<T>(IResponse<T> response)
        {
            if (!response.IsSuccess
                && !string.IsNullOrWhiteSpace(response.Message))
            {
                RaiseAlert(response.Message);
                return default;
            }
            else
            {
                return response.Result;
            }
        }

        protected void RaiseAlert(string message)
        {
            var alert = new Alert
            {
                AlertType = Alerts.ERROR,
                Title = "Error",
                Message = message
            };

            NavigationManager?.NavigateTo(alert.Page);
        }

        protected void RaiseAuthorisationAlert(string message)
        {
            var alert = new Alert
            {
                AlertType = Alerts.ERROR,
                Title = "Access Denied",
                Message = message
            };

            NavigationManager?.NavigateTo(alert.Page);
        }
    }
}
