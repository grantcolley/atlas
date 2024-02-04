using Atlas.Blazor.Web.App.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Web.App.Base
{
    public class ModelPropertyRenderComponentBase<T> : AtlasComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }
    }
}
