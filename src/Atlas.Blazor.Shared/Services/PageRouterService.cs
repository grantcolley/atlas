using Atlas.Blazor.Shared.Interfaces;
using Atlas.Blazor.Shared.Models;

namespace Atlas.Blazor.Shared.Services
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
