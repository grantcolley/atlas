using Atlas.Blazor.Web.Interfaces;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;

namespace Atlas.Blazor.Web.OptionItems
{
    public class RoutesOptionItems : IOptionItems
    {
        private static List<OptionItem>? _routes;
        private readonly IAtlasRoutesService _atlasRoutesService;

        public RoutesOptionItems(IAtlasRoutesService atlasRoutesService)
        {
            _atlasRoutesService = atlasRoutesService;
        }

        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            if (_routes != null) return _routes;

            IEnumerable<string> routes = [.. _atlasRoutesService.GetRoutes()
                .Where(s => !s.Contains("/alert/"))
                .Select(s => s.Remove(0, 1))
                .Order()];

            _routes = [new OptionItem(), .. routes.Select(r => new OptionItem { Id = r, Display = r })];

            return _routes;
        }
    }
}
