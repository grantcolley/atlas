using Atlas.Blazor.Shared.Interfaces;
using Atlas.Core.Dynamic;
using System.Linq.Expressions;

namespace Atlas.Blazor.Shared.Render
{
    public class ModelPropertyRender<T> : IModelPropertyRender<T> where T : class, new()
    {
        private readonly PropertyRender<T> _propertyRender;

        public ModelPropertyRender(T model, PropertyRender<T> propertyRender)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _propertyRender = propertyRender ?? throw new ArgumentNullException(nameof(propertyRender));

            DynamicType = DynamicTypeHelper.Get<T>();

            ConstantExpression constantExpression = Expression.Constant(Model);
            MemberExpression = Expression.Property(constantExpression, propertyRender.PropertyName);
        }

        public T Model { get; }
        public bool ReadOnly { get; }
        public DynamicType<T> DynamicType { get; }
        public MemberExpression MemberExpression { get; }

        public string PropertyName { get { return _propertyRender.PropertyName; } }
        public int Order { get { return _propertyRender.Order; } set { _propertyRender.Order = value; } }
        public string? Label { get { return _propertyRender.Label; } set { _propertyRender.Label = value; } }
        public string? Tooltip { get { return _propertyRender.Tooltip; } set { _propertyRender.Tooltip = value; } }
        public Dictionary<string, string> Parameters { get { return _propertyRender.Parameters; } set { _propertyRender.Parameters = value; } }
        public Type? ComponentType { get { return _propertyRender.ComponentType; } set { _propertyRender.ComponentType = value; } }

        public object? GetValue()
        {
            return DynamicType.GetValue(Model, PropertyName);
        }

        public void SetValue(object? value)
        {
            DynamicType.SetValue(Model, PropertyName, value);
        }
    }
}
