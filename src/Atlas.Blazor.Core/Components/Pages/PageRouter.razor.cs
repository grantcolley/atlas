using Atlas.Blazor.Core.Base;
using Atlas.Blazor.Core.Interfaces;
using Atlas.Blazor.Core.Models;
using Atlas.Core.Attributes;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Atlas.Blazor.Core.Pages
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
        public bool DataLoaded { get; set; }

        protected PageArgs? _pageArgs = default;

        public string? GetAccessToken()
        {
            return _pageArgs?.TokenProvider?.AccessToken;
        }

        protected override async Task OnInitializedAsync()
        {
            Debug.WriteLine($"{GetType().Name} - OnInitializedAsync START");

            await base.OnInitializedAsync();

            if (!string.IsNullOrWhiteSpace(PageCode)
                && PageRouterService != null)
            {
                _pageArgs = PageRouterService.GetPageArgs(PageCode);

                if (_pageArgs != null)
                {
                    _pageArgs.TokenProvider = LocalTokenProvider;

                    if (Id.HasValue)
                    {
                        _pageArgs.SetModelInstanceId(Id.Value);
                    }
                }
            }

            Debug.WriteLine($"{GetType().Name} - OnInitializedAsync END");
        }

        //protected override async Task OnParametersSetAsync()
        //{
        //    Debug.WriteLine($"{GetType().Name} - OnParametersSetAsync START");

        //    await base.OnParametersSetAsync().ConfigureAwait(false);

        //    if(ResetBreadcrumbRoot.HasValue
        //        && ResetBreadcrumbRoot.Value)
        //    {
        //        if (StateNotificationService != null)
        //        {
        //            var breadcrumb = new Breadcrumb
        //            {
        //                BreadcrumbAction = BreadcrumbAction.Home
        //            };

        //            await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
        //                .ConfigureAwait(false);
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(PageCode)
        //        && PageRouterService != null)
        //    {
        //        _pageArgs = PageRouterService.GetPageArgs(PageCode);

        //        if (_pageArgs != null)
        //        {
        //            if(Id.HasValue)
        //            {
        //                _pageArgs.SetModelInstanceId(Id.Value);
        //            }
        //        }
        //    }

        //    Debug.WriteLine($"{GetType().Name} - OnParametersSetAsync END");
        //}
    }
}
