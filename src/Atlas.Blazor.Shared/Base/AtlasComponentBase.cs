using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Interfaces;
using Atlas.Blazor.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public abstract class AtlasComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IStateNotificationService? StateNotificationService { get; set; }

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
