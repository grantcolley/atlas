using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Components.Generic
{
    public abstract class GenericComponentBase<T> : ComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }

        protected RenderFragment RenderView() => __builder =>
        {
            if(ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            string genericComponentName = ModelPropertyRender.Parameters[ElementParams.GENERIC_COMPONENT_NAME];

            var type = typeof(T).GetType();
            var genericComponent = Type.GetType(genericComponentName);

            if (genericComponent == null) throw new NullReferenceException(nameof(genericComponent));

            var genericType = genericComponent.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "ModelPropertyRender", ModelPropertyRender);
            __builder.CloseComponent();
        };
    }
}
