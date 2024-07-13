using Atlas.Blazor.Web.Constants;

namespace Atlas.Blazor.Web.Models
{
    public class Alert
    {
        public string? AlertType { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public string Route
        {
            get
            {
                return $"/{Alerts.ALERT_PAGE}/{AlertType ?? string.Empty}/{Title ?? string.Empty}/{Message ?? string.Empty}";
            }
        }
    }
}
