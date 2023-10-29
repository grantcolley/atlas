using Atlas.Blazor.UI.Constants;
using Atlas.Blazor.UI.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.UI.Base
{
    public class GenericPageArgsBase : AtlasComponentBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }

        [Parameter]
        public PageArgs? PageArgs { get; set; }

        protected async Task SendBreadcrumbAsync(BreadcrumbAction breadcrumbAction, string? text = null, string? href = null)
        {
            if (breadcrumbAction == BreadcrumbAction.Add
                && breadcrumbAction == BreadcrumbAction.Update
                && (string.IsNullOrWhiteSpace(text)
                ||(string.IsNullOrWhiteSpace(href))))
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
