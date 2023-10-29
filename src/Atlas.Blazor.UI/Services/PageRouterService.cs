using Atlas.Blazor.UI.Interfaces;
using Atlas.Blazor.UI.Models;

namespace Atlas.Blazor.UI.Services
{
    public class PageRouterService : IPageRouterService
    {
        private readonly Dictionary<string, PageArgs> _pageArgs = new();

        public PageRouterService(IEnumerable<PageArgs> pageArgs) 
        {
            foreach(PageArgs arg in pageArgs)
            {
                _pageArgs.Add(arg.PageCode, arg);
            }
        }

        public PageArgs GetPageArgs(string pageCode)
        {
            return _pageArgs[pageCode];
        }
    }
}
