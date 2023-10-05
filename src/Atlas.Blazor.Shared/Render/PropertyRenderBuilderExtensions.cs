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

        public static IPropertyRenderBuilder<T> WithParameters<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string parameters)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(parameters)) throw new ArgumentNullException(nameof(parameters));

            propertyRenderBuilder.PropertyRender.Tooltip = parameters;

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
