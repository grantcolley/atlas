namespace Atlas.Blazor.Components.Models
{
    public record Breadcrumb
    {
        public string? Text { get; set; }
        public string? Href { get; set; }
        public bool ResetAfterHome { get; set; }
        public bool ResetToHome { get; set; }
    }
}
