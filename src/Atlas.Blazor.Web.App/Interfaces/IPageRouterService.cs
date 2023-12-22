using Atlas.Blazor.Web.App.Models;

namespace Atlas.Blazor.Web.App.Interfaces
{
    public interface IPageRouterService
    {
        PageArgs GetPageArgs(string pageCode);
    }
}
