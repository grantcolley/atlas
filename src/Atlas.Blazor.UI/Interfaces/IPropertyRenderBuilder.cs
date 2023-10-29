using Atlas.Blazor.UI.Render;

namespace Atlas.Blazor.UI.Interfaces
{
    public interface IPropertyRenderBuilder<T> where T : class, new()
    {
        PropertyRender<T> PropertyRender { get; }
    }
}
