using Atlas.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Atlas.Core.Dynamic
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
        public int Order { get { return _propertyRender.Order; } }
        public string? Label { get { return _propertyRender.Label; } }
        public string? Tooltip { get { return _propertyRender.Tooltip; } }
        public string? Parameters { get { return _propertyRender.Parameters; } }
        public Type ComponentType { get { return _propertyRender.ComponentType; } }

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
