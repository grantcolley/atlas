using Atlas.Blazor.UI.Interfaces;

namespace Atlas.Blazor.UI.Render
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
        public Dictionary<string, string> Parameters { get; set; } = new();
        public Type? ComponentType { get; set; }
        public Type? GenericType { get; set; }
    }
}
