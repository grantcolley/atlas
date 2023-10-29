using Atlas.Blazor.UI.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.UI.Components.Generic
{
    public abstract class GenericModelPropertyRolloutBase<T, TRender> : ComponentBase 
        where T : class, new()
        where TRender : ModelRender<T>, new()
    {
        [Parameter]
        public T? Model { get; set; }

        protected IEnumerable<ModelPropertyRender<T>>? _modelPropertyRenders;
        protected TRender _render = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            _render.Configure();           
        }

        protected override void OnParametersSet()
        {
            if (Model == null) throw new ArgumentNullException(nameof(Model));

            _modelPropertyRenders = _render.GetModelPropertyRenders(Model);

            base.OnParametersSet();
        }

        protected RenderFragment RenderView(ModelPropertyRender<T> modelPropertyRender) => __builder =>
        {
            if (modelPropertyRender == null) throw new ArgumentNullException(nameof(modelPropertyRender));
            if (modelPropertyRender.ComponentType == null) throw new NullReferenceException(nameof(modelPropertyRender.ComponentType));

            __builder.OpenComponent(1, modelPropertyRender.ComponentType);
            __builder.AddAttribute(2, "ModelPropertyRender", modelPropertyRender);
            __builder.CloseComponent();
        };
    }
}
