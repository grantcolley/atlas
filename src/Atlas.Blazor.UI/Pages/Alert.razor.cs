using Atlas.Blazor.UI.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Atlas.Blazor.UI.Pages
{
    public partial class Alert : ComponentBase
    {
        [Parameter]
        public string? AlertType { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Message { get; set; }

        protected Severity _severity;

        protected override void OnInitialized()
        {
            _severity = AlertType switch
            {
                Alerts.NORMAL => Severity.Normal,
                Alerts.INFO => Severity.Info,
                Alerts.SUCCESS => Severity.Success,
                Alerts.WARNING => Severity.Warning,
                Alerts.ERROR => Severity.Error,
                _ => Severity.Normal,
            };

            base.OnInitialized();
        }
    }
}
