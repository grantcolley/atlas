using Atlas.Blazor.Core.Models;

namespace Atlas.Blazor.Core.Interfaces
{
    public interface IPageRouterService
    {
        PageArgs GetPageArgs(string pageCode);
    }
}
