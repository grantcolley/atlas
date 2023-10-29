using MudBlazor;

namespace Atlas.Blazor.UI.Interfaces
{
    public interface IDialogService
    {
        Task<DialogResult> ShowAsync(string title, string message, string buttonText, bool closeButton, Color color, bool scrollable);
    }
}
