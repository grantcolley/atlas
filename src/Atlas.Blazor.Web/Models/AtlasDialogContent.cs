namespace Atlas.Blazor.Web.Models
{
    public class AtlasDialogContent
    {
        public string? Title { get; set; }
        public bool ShowTitle { get; set; } = true;
        public List<string> Messages { get; set; } = [];
        public AtlasDialogType DialogType { get; set; } = AtlasDialogType.Ok;
        public AtlasDialogSelection Selected { get; set; } = AtlasDialogSelection.None;
        public bool ShowSecondaryButton { get; set; } = false;
        public string? Height { get; set; } = "320px";
        public string? Width { get; set; } = "500px";
        public bool? Modal { get; set; } = true;
        public bool PreventScroll { get; set; } = false;
    }
}