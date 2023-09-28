using Atlas.Blazor.Shared.Base;

namespace Atlas.Blazor.Shared.Components.Mud
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
