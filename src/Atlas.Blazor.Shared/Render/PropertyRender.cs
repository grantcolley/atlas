﻿using Atlas.Blazor.Shared.Interfaces;

namespace Atlas.Blazor.Shared.Render
{
    public class PropertyRender<T> : IPropertyRender<T> where T : class, new()
    {
        public PropertyRender(string propertyName)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        public string PropertyName { get; }

        public int Order { get; set; }
        public string? Label { get; set; }
        public string? Tooltip { get; set; }
        public string? Parameters { get; set; }
        public Type? ComponentType { get; set; }
    }
}