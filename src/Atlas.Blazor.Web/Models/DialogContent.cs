namespace Atlas.Blazor.Web.Models
{
    public class DialogContent
    {
        public string? Title { get; set; }
        public bool ShowTitle { get; set; } = true;
        public List<string> Messages { get; set; } = [];
        public object? Result { get; set; }
        public string? PrimaryButtonText { get; set; } = "Ok";
        public bool HasSecondaryButton { get; set; } = false;
        public string? SecondaryActionText { get; set; } = "Cancel";
        public string? Height { get; set; } = "320px";
        public string? Width { get; set; } = "500px";
        public bool? Modal { get; set; } = true;
        public bool PreventScroll { get; set; } = false;
    }
}