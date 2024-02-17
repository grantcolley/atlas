using Atlas.Blazor.Core.Interfaces;

namespace Atlas.Blazor.Core.Render
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
