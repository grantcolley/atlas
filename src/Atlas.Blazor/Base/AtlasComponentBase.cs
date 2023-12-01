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

        protected async Task SendBreadcrumbAsync(BreadcrumbAction breadcrumbAction, string? text = null, string? href = null)
        {
            if (breadcrumbAction == BreadcrumbAction.Add
                && breadcrumbAction == BreadcrumbAction.Update
                && (string.IsNullOrWhiteSpace(text)
                || (string.IsNullOrWhiteSpace(href))))
            {
                return;
            }

            if (StateNotificationService == null) throw new NullReferenceException(nameof(StateNotificationService));
            if (NavigationManager == null) throw new NullReferenceException(nameof(NavigationManager));

            if (breadcrumbAction == BreadcrumbAction.Add)
            {
                href = NavigationManager.Uri.Remove(0, NavigationManager.BaseUri.Length - 1);
            }

            var breadcrumb = new Breadcrumb
            {
                Text = text,
                Href = href,
                BreadcrumbAction = breadcrumbAction
            };

            await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
                .ConfigureAwait(false);
        }
    }
}
