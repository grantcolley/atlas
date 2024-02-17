using Atlas.Blazor.Core.Render;

namespace Atlas.Blazor.Core.Interfaces
{
    public interface IPropertyRenderBuilder<T> where T : class, new()
    {
        PropertyRender<T> PropertyRender { get; }
    }
}
