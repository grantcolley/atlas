using Atlas.Blazor.UI.Render;
using Atlas.Core.Dynamic;
using System.Linq.Expressions;

namespace Atlas.Blazor.UI.Interfaces
{
    public interface IModelPropertyRender<T> : IPropertyRender<T>
    {
        T Model { get; }
        bool ReadOnly { get; }
        DynamicType<T> DynamicType { get; }
        MemberExpression MemberExpression { get; }
    }
}
