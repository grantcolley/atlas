using Atlas.Blazor.Components.Constants;
using Atlas.Blazor.Components.Interfaces;
using Atlas.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Atlas.Blazor.Components.Common
{
    public abstract class BreadcrumbsBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateNotification? StateNotification { get; set; }

        protected List<BreadcrumbItem> _breadcrumbs = new();

        protected string? _uniqueId;

        public void Dispose()
        {
            if(StateNotification != null)
            {
                StateNotification.Deregister(StateNotifications.BREADCRUMBS);
            }

            GC.SuppressFinalize(this);
        }

        protected override Task OnInitializedAsync()
        {
            if (StateNotification == null)
            {
                throw new NullReferenceException(nameof(StateNotification));
            }

            StateNotification.Register(StateNotifications.BREADCRUMBS, BreadcrumbNotification);

            _breadcrumbs.Add(new BreadcrumbItem("Home", href: "/", icon: Icons.Material.Filled.Home));

            _uniqueId = Guid.NewGuid().ToString();

            return base.OnInitializedAsync();
        }

        private async Task BreadcrumbNotification(object arg)
        {
            var breadcrumb = arg as Breadcrumb;

            if (breadcrumb != null)
            {
                if (breadcrumb.ResetToHome)
                {
                    _breadcrumbs.RemoveRange(1, _breadcrumbs.Count - 1);
                }
                else
                {
                    if (_breadcrumbs.Count > 1
                        && breadcrumb.ResetAfterHome)
                    {
                        _breadcrumbs.RemoveRange(1, _breadcrumbs.Count - 1);
                    }

                    var lastBreadcrumb = _breadcrumbs.Last();

                    if (breadcrumb.Text != null
                        && !lastBreadcrumb.Text.Equals(breadcrumb.Text))
                    {
                        _breadcrumbs.Add(new BreadcrumbItem(breadcrumb.Text, breadcrumb.Href));
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