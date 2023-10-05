using Atlas.Blazor.Shared.Interfaces;

namespace Atlas.Blazor.Shared.Render
{
    public class PropertyRenderBuilder<T> : IPropertyRenderBuilder<T> where T : class, new()
    {
        public PropertyRenderBuilder(PropertyRender<T> propertyRender)
        {
            PropertyRender = propertyRender;
        }

        public PropertyRender<T> PropertyRender { get; private set; }
    }
}
