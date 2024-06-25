using Atlas.Blazor.Web.Models;

namespace Atlas.Blazor.Web.Interfaces
{
    public interface IAtlasDialogService
    {
        Task<string?> ShowIconsDialogAsync(string? icon);
        Task<AtlasDialogContent?> ShowDialogAsync(string title, string message);
        Task<AtlasDialogContent?> ShowDialogAsync(string title, string message, AtlasDialogType dialogType);
        Task<AtlasDialogContent?> ShowDialogAsync(string title, List<string> messages, AtlasDialogType dialogType);
        Task<AtlasDialogContent?> ShowDialogAsync(AtlasDialogContent dialogContent);
    }
}
