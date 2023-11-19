using Atlas.Blazor.Models;

namespace Atlas.Blazor.Interfaces
{
    public interface IPageRouterService
    {
        PageArgs GetPageArgs(string pageCode);
    }
}
