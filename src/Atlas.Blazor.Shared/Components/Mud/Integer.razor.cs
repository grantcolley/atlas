using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using System.Linq.Expressions;

namespace Atlas.Blazor.Shared.Components.Mud
{
    public class IntegerBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        protected int? maxLength = null;
        protected int? min = null;
        protected int? max = null;

        protected override Task OnInitializedAsync()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            if (ModelPropertyRender.Parameters.TryGetValue(ElementParams.MAX_LENGTH, out string? maxLengthArg))
            {
                maxLength = int.Parse(maxLengthArg);
            }

            if(ModelPropertyRender.Parameters.TryGetValue(ElementParams.MIN, out string? minArg))
            {
                min = int.Parse(minArg);
            }

            if(ModelPropertyRender.Parameters.TryGetValue(ElementParams.MAX, out string? maxArg))
            {
                max = int.Parse(maxArg);
            }

            return base.OnInitializedAsync();
        }

        public int MaxLength { get { return maxLength ?? int.MaxValue; } }
        public int Max { get { return max ?? int.MaxValue; } }
        public int Min { get { return min ?? int.MinValue; } }

        public Expression<Func<int>> FieldExpression
        {
            get
            {
                if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

                return Expression.Lambda<Func<int>>(ModelPropertyRender.MemberExpression);
            }
        }

        public int PropertyValue
        {
            get
            {
                return (int)(ModelPropertyRender?.GetValue() ?? throw new NullReferenceException(nameof(ModelPropertyRender)));
            }
            set
            {
                ModelPropertyRender?.SetValue(value);
            }
        }
    }
}
