using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.UI.Components.Shared
{
    public partial class Loading : ComponentBase
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Message { get; set; }
    }
}
