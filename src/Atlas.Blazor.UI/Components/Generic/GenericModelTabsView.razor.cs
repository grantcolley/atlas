using Atlas.Blazor.UI.Base;
using Atlas.Blazor.UI.Render;

namespace Atlas.Blazor.UI.Components.Generic
{
    public abstract class GenericModelTabsViewBase<T, TRender> : GenericModelViewBase<T, TRender>
        where T : class, new()
        where TRender : ModelRender<T>, new()
    {
        protected object activePage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

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

        protected void SetActivePage(object page)
        {
            activePage = page;
        }

        private void SetActivePage()
        {
            //if (DynamicModel != null)
            //{
            //    if (activePage != null)
            //    {
            //        activePage = DynamicModel.RootContainers.FirstOrDefault(c => c.ContainerId.Equals(activePage.ContainerId));
            //    }

            //    activePage ??= DynamicModel.RootContainers.First();
            //}
        }
    }
}
