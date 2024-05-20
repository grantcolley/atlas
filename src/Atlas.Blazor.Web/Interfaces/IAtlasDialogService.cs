using Atlas.Blazor.Web.Models;

namespace Atlas.Blazor.Web.Interfaces
{
    public interface IAtlasDialogService
    {
        Task<DialogContent?> ShowDialogAsync(string title, List<string> messages);
        Task<DialogContent?> ShowDialogAsync(DialogContent dialogContent);
    }
}
