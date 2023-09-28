using System;

namespace Atlas.Core.Interfaces
{
    public interface IPropertyRender<T>
    {
        int Order { get; }
        string PropertyName { get; }
        string? Label { get; }
        string? Tooltip { get; }
        string? Parameters { get; }
        Type ComponentType { get; }
    }
}
