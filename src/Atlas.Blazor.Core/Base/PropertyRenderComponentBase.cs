using Atlas.Blazor.Core.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Core.Base
{
    public class ModelPropertyRenderComponentBase<T> : AtlasComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }
    }
}
