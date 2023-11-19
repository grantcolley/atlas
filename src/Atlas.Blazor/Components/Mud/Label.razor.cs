using Atlas.Blazor.Base;

namespace Atlas.Blazor.Components.Mud
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
