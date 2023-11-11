using Atlas.Blazor.UI.Interfaces;

namespace Atlas.Blazor.UI.Render
{
    public class PropertyRender<T> : IPropertyRender<T> where T : class, new()
    {
        public PropertyRender(string propertyName, ModelRender<T> modelRender)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            ModelRender = modelRender ?? throw new ArgumentNullException(nameof(modelRender));
        }

        public string PropertyName { get; }
        public ModelRender<T> ModelRender { get; }
        public int Order { get; set; }
        public string? Label { get; set; }
        public string? Tooltip { get; set; }
        public string? ContainerCode { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
        public Type? ComponentType { get; set; }
        public Type? GenericType { get; set; }
    }
}
