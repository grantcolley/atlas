using Atlas.Blazor.Render;
using Atlas.Core.Dynamic;
using System.Linq.Expressions;

namespace Atlas.Blazor.Interfaces
{
    public interface IModelPropertyRender<T> : IPropertyRender<T>
    {
        T Model { get; }
        bool ReadOnly { get; }
        DynamicType<T> DynamicType { get; }
        MemberExpression MemberExpression { get; }
    }
}
