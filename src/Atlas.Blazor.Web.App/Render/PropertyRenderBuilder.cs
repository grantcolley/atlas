using Atlas.Blazor.Web.App.Interfaces;

namespace Atlas.Blazor.Web.App.Render
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
