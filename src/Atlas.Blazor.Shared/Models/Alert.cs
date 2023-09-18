using Atlas.Blazor.Shared.Constants;

namespace Atlas.Blazor.Shared.Models
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
