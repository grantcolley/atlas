using Atlas.Blazor.Web.Interfaces;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;

namespace Atlas.Blazor.Web.OptionItems
{
    public class RoutesOptionItems : IOptionItems
    {
        private static IEnumerable<OptionItem>? _routes;
        private readonly IAtlasRoutesService _atlasRoutesService;

        public RoutesOptionItems(IAtlasRoutesService atlasRoutesService)
        {
            _atlasRoutesService = atlasRoutesService;
        }

        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            if (_routes != null) return _routes;

            _routes = [.. _atlasRoutesService.GetRoutes()
                .Select(r => new OptionItem { Id = r, Display = r })
                .OrderBy(oi => oi.Display)];

            return _routes;
        }
    }
}
