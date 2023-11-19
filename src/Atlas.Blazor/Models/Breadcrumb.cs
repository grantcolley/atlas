namespace Atlas.Blazor.Models
{
    public record Breadcrumb
    {
        public string? Text { get; set; }
        public string? Href { get; set; }
        public BreadcrumbAction BreadcrumbAction { get; set; }
    }

    public enum BreadcrumbAction
    {
        Add,
        Home,
        Update,
        RemoveLast
    }
}
