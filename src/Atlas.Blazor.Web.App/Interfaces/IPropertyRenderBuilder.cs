using Atlas.Blazor.Web.App.Render;

namespace Atlas.Blazor.Web.App.Interfaces
{
    public interface IPropertyRenderBuilder<T> where T : class, new()
    {
        PropertyRender<T> PropertyRender { get; }
    }
}
