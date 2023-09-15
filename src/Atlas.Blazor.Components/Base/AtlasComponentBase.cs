using Atlas.Blazor.Components.Constants;
using Atlas.Blazor.Components.Interfaces;
using Atlas.Blazor.Components.Models;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Components.Base
{
    public abstract class AtlasComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IStateNotificationService? StateNotificationService { get; set; }

        [Inject]
        public IComponentArgsRequests? ComponentArgsRequests { get; set; }

        [Parameter]
        public string? ComponentCode { get; set; }

        protected ComponentArgs? _componentArgs = default;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(ComponentCode)
                && NavigationManager != null
                && StateNotificationService != null
                && ComponentArgsRequests != null)
            {
                _componentArgs = await ComponentArgsRequests.GetComponentArgsAsync(ComponentCode)
                    .ConfigureAwait(false);

                if (_componentArgs != null)
                {
                    var breadcrumb = new Breadcrumb
                    {
                        Text = _componentArgs.DisplayName,
                        Href = NavigationManager.Uri.Remove(0, NavigationManager.BaseUri.Length - 1),
                        ResetAfterHome = _componentArgs.NavigateResetBreadcrumb
                    };

                    await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
                        .ConfigureAwait(false);
                }
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
