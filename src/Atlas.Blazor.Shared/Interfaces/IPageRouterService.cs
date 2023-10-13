using Atlas.Blazor.Shared.Models;

namespace Atlas.Blazor.Shared.Interfaces
{
    public interface IPageRouterService
    {
        PageArgs GetPageArgs(string pageCode);
    }
}
