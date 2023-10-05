using Atlas.Blazor.Shared.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Atlas.Blazor.Shared.Render
{
    public abstract class ModelRender<T> where T : class, new()
    {
        private readonly List<PropertyRender<T>> _propertyRenders = new();

        public abstract void Configure();

        protected IPropertyRenderBuilder<T> RenderProperty<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            MemberInfo? memberInfo = expression.GetMemberInfo();

            if (memberInfo == null) throw new NullReferenceException(nameof(memberInfo));

            PropertyRender<T> propertyRender = new(memberInfo.Name);

            _propertyRenders.Add(propertyRender);

            return new PropertyRenderBuilder<T>(propertyRender);
        }

        public IEnumerable<ModelPropertyRender<T>> GetModelPropertyRenders(T model)
        {
            List<ModelPropertyRender<T>> modelPropertyRenders = new();

            foreach (PropertyRender<T> propertyRender in _propertyRenders)
            {
                modelPropertyRenders.Add(new ModelPropertyRender<T>(model, propertyRender));
            }

            return modelPropertyRenders;
        }
    }
}
