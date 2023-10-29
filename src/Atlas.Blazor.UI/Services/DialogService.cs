using Atlas.Blazor.UI.Components.Shared;
using MudBlazor;

namespace Atlas.Blazor.UI.Services
{
    public class DialogService : Interfaces.IDialogService
    {
        private readonly MudBlazor.IDialogService dialogService;

        public DialogService(MudBlazor.IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public Task<DialogResult> ShowAsync(
            string title, string message, string buttonText,
            bool closeButton, Color color, bool scrollable)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", message },
                { "ButtonText", buttonText },
                { "Color", color }
            };

            var options = new DialogOptions()
            {
                CloseButton = closeButton,
                MaxWidth = MaxWidth.ExtraSmall
            };

            if (scrollable)
            {
                return dialogService.Show<ScrollableDialog>(title, parameters, options).Result;
            }
            else
            {
                return dialogService.Show<ScrollableDialog>(title, parameters, options).Result;
            }
        }
    }
}
