using Atlas.Blazor.Components.Constants;

namespace Atlas.Blazor.Components.Models
{
    public class Alert
    {
        public string? AlertType { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public string Page
        {
            get
            {
                return $"/{Alerts.ALERT_PAGE}/{AlertType ?? string.Empty}/{Title ?? string.Empty}/{Message ?? string.Empty}";
            }
        }
    }
}
