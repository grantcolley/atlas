using Atlas.Blazor.Shared.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class ModelPropertyRenderComponentBase<T> : ComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }
    }
}
