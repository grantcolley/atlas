using Atlas.Blazor.Web.App.Interfaces;
using Atlas.Blazor.Web.App.Models;
using System.Collections.Generic;

namespace Atlas.Blazor.Web.App.Services
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
