using Atlas.Blazor.Core.Components.Generic;
using Atlas.Blazor.Core.Constants;
using Atlas.Blazor.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Atlas.Blazor.Core.Render
{
    public static class PropertyRenderBuilderExtensions
    {
        public static IPropertyRenderBuilder<T> RenderAsDropdownGeneric<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string genericModelDisplayField)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(genericModelDisplayField)) throw new ArgumentNullException(nameof(genericModelDisplayField));
            if (propertyRenderBuilder.PropertyRender.GenericType == null) throw new NullReferenceException(nameof(propertyRenderBuilder.PropertyRender.GenericType));

            propertyRenderBuilder.PropertyRender.Parameters.Add(ElementParams.GENERIC_MODEL_DISPLAY_FIELD, genericModelDisplayField);

            return propertyRenderBuilder.RenderAsGenericComponent<T>(Element.DropdownGeneric);
        }

        public static IPropertyRenderBuilder<T> RenderAs<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string componentType)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(componentType)) throw new ArgumentNullException(nameof(componentType));

            Type type = typeof(T);

            Type? genericType = Type.GetType(componentType)?.MakeGenericType(new[] { type });

            propertyRenderBuilder.PropertyRender.ComponentType = genericType ?? throw new NullReferenceException(nameof(genericType));

            return propertyRenderBuilder;
        }

        public static IPropertyRenderBuilder<T> InContainer<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string containerCode)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(containerCode)) throw new ArgumentNullException(nameof(containerCode));

            if(propertyRenderBuilder.PropertyRender.ModelRender.ContainerExists(containerCode))
            {
                propertyRenderBuilder.PropertyRender.ContainerCode = containerCode;
            }
            else
            {
                throw new NullReferenceException($"{containerCode} does not exist");
            }

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

        private static IPropertyRenderBuilder<T> RenderAsGenericComponent<T>(this IPropertyRenderBuilder<T> propertyRenderBuilder, string genericComponentType)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(genericComponentType)) throw new ArgumentNullException(nameof(genericComponentType));

            propertyRenderBuilder.PropertyRender.Parameters.Add(ElementParams.GENERIC_COMPONENT_NAME, genericComponentType);

            Type? genericType = typeof(GenericComponent<T>);

            propertyRenderBuilder.PropertyRender.ComponentType = genericType ?? throw new NullReferenceException(nameof(genericType));

            return propertyRenderBuilder;
        }
    }
}
