using Atlas.Blazor.UI.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.UI.Base
{
    public class ModelPropertyRenderComponentBase<T> : AtlasComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }
    }
}
