using Atlas.Blazor.Web.Components;
using Atlas.Blazor.Web.Components.Support;
using Atlas.Blazor.Web.Interfaces;
using Atlas.Blazor.Web.Models;
using Atlas.Core.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Atlas.Blazor.Web.Services
{
    public class AtlasDialogService(IDialogService dialogService) : IAtlasDialogService
    {
        private readonly IDialogService _dialogService = dialogService;

        public Task<AtlasDialogContent?> ShowDialogAsync(string title, string message)
        {
            return ShowDialogAsync(title, [message], AtlasDialogType.Ok);
        }

        public Task<AtlasDialogContent?> ShowDialogAsync(string title, string message, AtlasDialogType dialogType)
        {
            return ShowDialogAsync(title, [message], dialogType);
        }

        public Task<AtlasDialogContent?> ShowDialogAsync(string title, List<string> messages, AtlasDialogType dialogType)
        {
            AtlasDialogContent content = new()
            {
                Title = title,
                DialogType = dialogType,
                ShowSecondaryButton = dialogType != AtlasDialogType.Ok
            };

            content.Messages.AddRange(messages);

            return ShowDialogAsync(content);
        }

        public async Task<AtlasDialogContent?> ShowDialogAsync(AtlasDialogContent dialogContent)
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

            AtlasDialogContent? dialogResult = default;

            if (result.Data is not null)
            {
                dialogResult = result.Data as AtlasDialogContent;
            }

            return dialogResult;
        }

        public async Task ShowLogDialogAsync(Log log)
        {
            ArgumentNullException.ThrowIfNull(log, nameof(log));

            DialogParameters parameters = new()
            {
                Title = string.IsNullOrWhiteSpace(log.Message) ? "Log Entry" : log.Message,
                Width = "500px",
                Height = "500px",
                Modal = false,
                PreventScroll = false
            };

            IDialogReference dialog = await _dialogService.ShowDialogAsync<LogDialog>(log, parameters);

            _ = await dialog.Result;
        }
    }
}
