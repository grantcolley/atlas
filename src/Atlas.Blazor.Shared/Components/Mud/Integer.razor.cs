using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using System.Linq.Expressions;

namespace Atlas.Blazor.Shared.Components.Mud
{
    public class IntegerBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        protected int? _maxLength = null;
        protected int? _min = null;
        protected int? _max = null;

        protected override Task OnInitializedAsync()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            if (ModelPropertyRender.Parameters.TryGetValue(ElementParams.MAX_LENGTH, out string? maxLengthArg))
            {
                _maxLength = int.Parse(maxLengthArg);
            }

            if(ModelPropertyRender.Parameters.TryGetValue(ElementParams.MIN, out string? minArg))
            {
                _min = int.Parse(minArg);
            }

            if(ModelPropertyRender.Parameters.TryGetValue(ElementParams.MAX, out string? maxArg))
            {
                _max = int.Parse(maxArg);
            }

            return base.OnInitializedAsync();
        }

        public int MaxLength { get { return _maxLength ?? int.MaxValue; } }
        public int Max { get { return _max ?? int.MaxValue; } }
        public int Min { get { return _min ?? int.MinValue; } }

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
