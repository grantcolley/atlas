using Atlas.Blazor.Render;

namespace Atlas.Blazor.Interfaces
{
    public interface IPropertyRenderBuilder<T> where T : class, new()
    {
        PropertyRender<T> PropertyRender { get; }
    }
}
