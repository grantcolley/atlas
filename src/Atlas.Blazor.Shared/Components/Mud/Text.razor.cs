using Atlas.Blazor.Shared.Base;
using System.Linq.Expressions;

namespace Atlas.Blazor.Shared.Components.Mud
{
    public abstract class TextBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        public Expression<Func<string?>> FieldExpression
        {
            get
            {
                if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

                return Expression.Lambda<Func<string?>>(ModelPropertyRender.MemberExpression);
            }
        }

        public string? PropertyValue
        {
            get
            {
                return ModelPropertyRender?.GetValue()?.ToString();
            }
            set
            {
                ModelPropertyRender?.SetValue(value);
            }
        }
    }
}
