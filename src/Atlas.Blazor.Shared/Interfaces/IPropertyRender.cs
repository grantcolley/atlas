namespace Atlas.Blazor.Shared.Interfaces
{
    public interface IPropertyRender<T>
    {
        string PropertyName { get; }
        int Order { get; set; }
        string? Label { get; set; }
        string? Tooltip { get; set; }
        string? Parameters { get; set; }
        Type? ComponentType { get; set; }
    }
}
