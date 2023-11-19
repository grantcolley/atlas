using Atlas.Blazor.Base;
using Atlas.Blazor.Constants;
using Atlas.Blazor.Interfaces;
using Atlas.Blazor.Models;
using Atlas.Core.Attributes;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Pages
{
    [NavigationPageAttribute]
    public abstract class PageRouterBase : AtlasComponentBase
    {
        [Inject]
        public IPageRouterService? PageRouterService { get; set; }

        [Parameter]
        public string? PageCode { get; set; }

        [Parameter]
        public int? Id { get; set; }

        [Parameter]
        public bool? ResetBreadcrumbRoot { get; set; }

        protected PageArgs? _pageArgs = default;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if(ResetBreadcrumbRoot.HasValue
                && ResetBreadcrumbRoot.Value)
            {
                if (StateNotificationService != null)
                {
                    var breadcrumb = new Breadcrumb
                    {
                        BreadcrumbAction = BreadcrumbAction.Home
                    };

                    await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
                        .ConfigureAwait(false);
                }
            }

            if (!string.IsNullOrWhiteSpace(PageCode)
                && PageRouterService != null)
            {
                _pageArgs = PageRouterService.GetPageArgs(PageCode);

                if (_pageArgs != null)
                {
                    if(Id.HasValue)
                    {
                        _pageArgs.SetModelInstanceId(Id.Value);
                    }
                }
            }
        }
    }
}
