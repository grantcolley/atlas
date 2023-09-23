using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Models;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Pages
{
    public abstract class PageRouterBase : AtlasComponentBase
    {
        [Inject]
        public IComponentArgsRequests? ComponentArgsRequests { get; set; }

        [Parameter]
        public string? ComponentCode { get; set; }

        [Parameter]
        public int? Id { get; set; }

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
                    if(Id.HasValue)
                    {
                        _componentArgs.ModelInstanceId = Id.Value;
                    }

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
    }
}
