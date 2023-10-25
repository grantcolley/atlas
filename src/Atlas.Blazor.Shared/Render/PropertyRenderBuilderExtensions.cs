using Atlas.Blazor.Shared.Components.Generic;
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Interfaces;

namespace Atlas.Blazor.Shared.Render
{
    public static class PropertyRenderBuilderExtensions
    {
        public static IPropertyRenderBuilder<T> RenderAs<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string componentType)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(componentType)) throw new ArgumentNullException(nameof(componentType));

            Type type = typeof(T);

            Type? genericType = Type.GetType(componentType)?.MakeGenericType(new[] { type });

            propertyRenderBuilder.PropertyRender.ComponentType = genericType ?? throw new NullReferenceException(nameof(genericType));

            return propertyRenderBuilder;
        }

        public static IPropertyRenderBuilder<T> RenderAsGenericComponent<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string genericComponentType)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(genericComponentType)) throw new ArgumentNullException(nameof(genericComponentType));

            propertyRenderBuilder.PropertyRender.Parameters.Add(ElementParams.GENERIC_COMPONENT_NAME, genericComponentType);

            Type type = typeof(T);

            Type? genericType = typeof(GenericComponent<T>).GetType()?.MakeGenericType(new[] { type });

            propertyRenderBuilder.PropertyRender.ComponentType = genericType ?? throw new NullReferenceException(nameof(genericType));

            return propertyRenderBuilder;
        }

        public static IPropertyRenderBuilder<T> WithLabel<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string label)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(label)) throw new ArgumentNullException(nameof(label));

            propertyRenderBuilder.PropertyRender.Label = label;

            return propertyRenderBuilder;
        }

        public static IPropertyRenderBuilder<T> WithTooltip<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string tooltip)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(tooltip)) throw new ArgumentNullException(nameof(tooltip));

            propertyRenderBuilder.PropertyRender.Tooltip = tooltip;

            return propertyRenderBuilder;
        }

        public static IPropertyRenderBuilder<T> WithParameters<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, Dictionary<string, string> parameters)
            where T : class, new()
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
            {
                propertyRenderBuilder.PropertyRender.Parameters.Add(parameter.Key, parameter.Value);
            }

            return propertyRenderBuilder;
        }

        public static IPropertyRenderBuilder<T> RenderOrder<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, int order)
            where T : class, new()
        {
            propertyRenderBuilder.PropertyRender.Order = order;

            return propertyRenderBuilder;
        }
    }
}
