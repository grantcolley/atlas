using Atlas.Blazor.Core.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Diagnostics;

namespace Atlas.Blazor.Core.Pages
{
    public abstract class AlertBase : ComponentBase
    {
        [Parameter]
        public string? AlertType { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Message { get; set; }

        protected MessageIntent _intent;

        protected override void OnInitialized()
        {
            Debug.WriteLine($"### {GetType().Name} - AlertBase - OnInitialized - START");

            _intent = AlertType switch
            {
                Alerts.INFO => MessageIntent.Info,
                Alerts.SUCCESS => MessageIntent.Success,
                Alerts.WARNING => MessageIntent.Warning,
                Alerts.ERROR => MessageIntent.Error,
                _ => MessageIntent.Info,
            };

            base.OnInitialized();

            Debug.WriteLine($"### {GetType().Name} - AlertBase - OnInitialized - END");
        }
    }
}
