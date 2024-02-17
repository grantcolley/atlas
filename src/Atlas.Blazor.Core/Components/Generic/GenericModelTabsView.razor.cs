using Atlas.Blazor.Core.Render;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlas.Blazor.Core.Components.Generic
{
    public abstract class GenericModelTabsViewBase<T, TRender> : GenericModelViewBase<T, TRender>
        where T : class, new()
        where TRender : ModelRender<T>, new()
    {
        protected IEnumerable<Container>? _containers { get; set; }
        protected Container? _activeContainer { get; set; }
        private TRender _render = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            _render.Configure();

            _containers = _render.GetContainers();

            SetActivePage();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SetActivePage(Container page)
        {
            _activeContainer = page;
        }

        private void SetActivePage()
        {
            if (_containers != null)
            {
                if (_activeContainer != null)
                {
                    _activeContainer = _containers.FirstOrDefault(c =>
                        !string.IsNullOrWhiteSpace(c.ContainerCode)
                        && c.ContainerCode.Equals(_activeContainer.ContainerCode));
                }

                _activeContainer ??= _containers.FirstOrDefault();
            }
        }
    }
}
