using Atlas.Blazor.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Base
{
    public class ModelPropertyRenderComponentBase<T> : AtlasComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }
    }
}
