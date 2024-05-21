using Atlas.Blazor.Web.Components;
using Atlas.Blazor.Web.Interfaces;
using Atlas.Blazor.Web.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Atlas.Blazor.Web.Services
{
    public class AtlasDialogService : IAtlasDialogService
    {
        private readonly IDialogService _dialogService;

        public AtlasDialogService(IDialogService dialogService) 
        {
            _dialogService = dialogService;
        }

        public Task<DialogContent?> ShowDialogAsync(string title, string message)
        {
            DialogContent content = new() { Title = title };
            content.Messages.Add(message);
            return ShowDialogAsync(content);
        }

        public Task<DialogContent?> ShowDialogAsync(string title, List<string> messages)
        {
            DialogContent content = new()
            {
                Title = title,
                Messages = messages
            };

            return ShowDialogAsync(content);
        }

        public async Task<DialogContent?> ShowDialogAsync(DialogContent dialogContent)
        {
            ArgumentNullException.ThrowIfNull(dialogContent, nameof(dialogContent));

            DialogParameters parameters = new()
            {
                Title = dialogContent.Title,
                Width = dialogContent.Width,
                Height = dialogContent.Height,
                Modal = dialogContent.Modal,
                PreventScroll = dialogContent.PreventScroll
            };

            IDialogReference dialog = await _dialogService.ShowDialogAsync<AtlasDialog>(dialogContent, parameters);
            DialogResult? result = await dialog.Result;

            DialogContent? dialogResult = default;

            if (result.Data is not null)
            {
                dialogResult = result.Data as DialogContent;
            }

            return dialogResult;
        }
    }
}
