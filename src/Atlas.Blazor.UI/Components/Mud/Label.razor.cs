using Atlas.Blazor.UI.Base;

namespace Atlas.Blazor.UI.Components.Mud
{
    public abstract class LabelBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        public string? PropertyValue
        {
            get
            {
                return ModelPropertyRender?.GetValue()?.ToString();
            }
        }
    }
}
