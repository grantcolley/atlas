using Atlas.Blazor.Shared.Render;

namespace Atlas.Blazor.Shared.Interfaces
{
    public interface IPropertyRenderBuilder<T> where T : class, new()
    {
        PropertyRender<T> PropertyRender { get; }
    }
}
