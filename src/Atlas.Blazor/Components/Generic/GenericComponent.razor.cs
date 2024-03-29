﻿using Atlas.Blazor.Components.Mud;
using Atlas.Blazor.Constants;
using Atlas.Blazor.Render;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Components.Generic
{
    public abstract class GenericComponentBase<T> : ComponentBase where T : class, new()
    {
        [Parameter]
        public ModelPropertyRender<T>? ModelPropertyRender { get; set; }

        protected RenderFragment RenderView() => __builder =>
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));
            if (ModelPropertyRender.GenericType == null) throw new NullReferenceException(nameof(ModelPropertyRender.GenericType));

            string genericComponentName = ModelPropertyRender.Parameters[ElementParams.GENERIC_COMPONENT_NAME];

            var genericComponent = Type.GetType(genericComponentName);

            if (genericComponent == null) throw new NullReferenceException(nameof(genericComponent));

            var genericType = genericComponent.MakeGenericType(new[] { ModelPropertyRender.DynamicType.DeclaringType, ModelPropertyRender.GenericType });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "ModelPropertyRender", ModelPropertyRender);
            __builder.CloseComponent();
        };
    }
}
