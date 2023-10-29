using Atlas.Blazor.UI.Constants;
using Atlas.Blazor.UI.Interfaces;
using Atlas.Blazor.UI.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Atlas.Blazor.UI.Components.Layout
{
    public abstract class BreadcrumbsBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateNotificationService? StateNotificationService { get; set; }

        protected List<BreadcrumbItem> _breadcrumbs = new();

        protected string? _uniqueId;

        public void Dispose()
        {
            StateNotificationService?.Deregister(StateNotifications.BREADCRUMBS);

            GC.SuppressFinalize(this);
        }

        protected override Task OnInitializedAsync()
        {
            if (StateNotificationService == null) throw new NullReferenceException(nameof(StateNotificationService));

            StateNotificationService.Register(StateNotifications.BREADCRUMBS, BreadcrumbNotification);

            _breadcrumbs.Add(new BreadcrumbItem("Home", href: "/", icon: Icons.Material.Filled.Home));

            _uniqueId = Guid.NewGuid().ToString();

            return base.OnInitializedAsync();
        }

        private async Task BreadcrumbNotification(object arg)
        {
            var breadcrumb = arg as Breadcrumb;

            if (breadcrumb != null)
            {
                if (breadcrumb.BreadcrumbAction.Equals(BreadcrumbAction.Home))
                {
                    _breadcrumbs.RemoveRange(1, _breadcrumbs.Count - 1);
                }
                else
                {
                    if (breadcrumb.BreadcrumbAction.Equals(BreadcrumbAction.RemoveLast)) 
                    {
                        if (_breadcrumbs.Count > 1)
                        {
                            _breadcrumbs.RemoveAt(_breadcrumbs.Count - 1);
                        }
                    }
                    else if (breadcrumb.Text != null
                        && breadcrumb.Href != null)
                    {
                        if (breadcrumb.BreadcrumbAction.Equals(BreadcrumbAction.Add))
                        {
                            int breadcrumbIndex = -1;

                            for (int i = 0; i < _breadcrumbs.Count; i++)
                            {
                                if (_breadcrumbs[i].Href != null
                                    && _breadcrumbs[i].Text == breadcrumb.Text
                                    && _breadcrumbs[i].Href == breadcrumb.Href)
                                {
                                    breadcrumbIndex = i;
                                    break;
                                }
                            }

                            if (breadcrumbIndex == -1)
                            {
                                _breadcrumbs.Add(new BreadcrumbItem(breadcrumb.Text, breadcrumb.Href));
                            }
                            else
                            {
                                int removeFromIndex = breadcrumbIndex + 1;
                                _breadcrumbs.RemoveRange(removeFromIndex, _breadcrumbs.Count - removeFromIndex);
                            }
                        }
                        else if (breadcrumb.BreadcrumbAction.Equals(BreadcrumbAction.Update))
                        {
                            if (_breadcrumbs.Count > 1)
                            {
                                _breadcrumbs.RemoveAt(_breadcrumbs.Count - 1);
                                _breadcrumbs.Add(new BreadcrumbItem(breadcrumb.Text, breadcrumb.Href));
                            }
                        }
                    }
                }

                await InvokeAsync(() =>
                {
                    StateHasChanged();
                }).ConfigureAwait(true);
            }
        }
    }
}