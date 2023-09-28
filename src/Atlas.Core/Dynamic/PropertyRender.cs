using Atlas.Core.Interfaces;
using System;

namespace Atlas.Core.Dynamic
{
    public class PropertyRender<T> : IPropertyRender<T> where T : class, new()
    {
        public PropertyRender(string propertyName, int order, string componentType,
            string? label = "", string? tooltip = "", string? parameters = "")
        {
            Type type = typeof(T);

            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

            if (string.IsNullOrWhiteSpace(componentType)) throw new ArgumentNullException(nameof(componentType));

            Type? genericType = Type.GetType(componentType)?.MakeGenericType(new[] { type });

            ComponentType = genericType ?? throw new NullReferenceException(nameof(genericType));

            Order = order;

            if (!string.IsNullOrEmpty(label)) 
            {
                Label = label;
            }

            if(!string.IsNullOrEmpty(tooltip)) 
            {
                Tooltip = tooltip;
            }

            if (!string.IsNullOrEmpty(parameters))
            {
                Parameters = parameters;
            }
        }

        public int Order { get; }
        public string PropertyName { get; }
        public string? Label { get; }
        public string? Tooltip { get; }
        public string? Parameters { get; }
        public Type ComponentType { get; }
    }
}
