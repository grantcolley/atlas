using Atlas.Blazor.UI.Models;

namespace Atlas.Blazor.UI.Interfaces
{
    public interface IPageRouterService
    {
        PageArgs GetPageArgs(string pageCode);
    }
}
