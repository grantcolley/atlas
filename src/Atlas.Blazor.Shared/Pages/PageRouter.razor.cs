using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Interfaces;
using Atlas.Blazor.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Pages
{
    public abstract class PageRouterBase : AtlasComponentBase
    {
        [Inject]
        public IPageRouterService? PageRouterService { get; set; }

        [Parameter]
        public string? PageCode { get; set; }

        [Parameter]
        public int? Id { get; set; }

        protected PageArgs? _pageArgs = default;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(PageCode)
                && NavigationManager != null
                && StateNotificationService != null
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
