using Atlas.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atlas.Core.Dynamic
{
    public static class ModelPropertyRenderHelper
    {
        private static readonly IDictionary<Type, object> _cache = new Dictionary<Type, object>();

        private static readonly object _cacheLock = new();

        public static IEnumerable<ModelPropertyRender<T>> GetModelPropertyRenders<T>(T model) where T : class, new()
        {
            IEnumerable<PropertyRender<T>> propertyRenders = GetPropertyRenders<T>();

            List<ModelPropertyRender<T>> modelPropertyRenders = new();

            foreach(PropertyRender<T> propertyRender in propertyRenders) 
            {
                modelPropertyRenders.Add(new ModelPropertyRender<T>(model, propertyRender));
            }

            return modelPropertyRenders;
        }

        private static IEnumerable<PropertyRender<T>> GetPropertyRenders<T>() where T : class, new()
        {
            lock (_cacheLock)
            {
                Type? t = typeof(T);

                if (_cache.TryGetValue(t, out object? result))
                {
                    return (IEnumerable<PropertyRender<T>>)result;
                }

                IEnumerable<PropertyRender<T>> propertyRenders = CreatePropertyRenders<T>();

                _cache.Add(t, propertyRenders);

                return propertyRenders;
            }
        }

        private static IEnumerable<PropertyRender<T>> CreatePropertyRenders<T>() where T : class, new()
        {
            DynamicType<T> dynamicType = DynamicTypeHelper.Get<T>();

            IEnumerable<PropertyInfo> propertyInfos = dynamicType.SupportedProperties
                .Where(p => p.GetCustomAttributes(typeof(ModelPropertyRenderAttribute), true).Any());

            List<PropertyRender<T>> propertyRenders = new();

            foreach (var propertyInfo in propertyInfos) 
            {
                object[]? modelPropertyRenderAttributes = propertyInfo.GetCustomAttributes(typeof(ModelPropertyRenderAttribute), true);

                if (modelPropertyRenderAttributes != null
                    && modelPropertyRenderAttributes.Length == 1)
                {
                    ModelPropertyRenderAttribute attribute = (ModelPropertyRenderAttribute)modelPropertyRenderAttributes[0];

                    if(!string.IsNullOrWhiteSpace(attribute.Component))
                    {
                        propertyRenders.Add(new PropertyRender<T>(propertyInfo.Name, attribute.Order, attribute.Component, attribute.Label, attribute.Tooltip));
                    }
                }
            }

            return propertyRenders.OrderBy(p => p.Order);
        }
    }
}
