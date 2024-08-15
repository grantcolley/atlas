using Atlas.Blazor.Web.Interfaces;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;

namespace Atlas.Blazor.Web.OptionItems
{
    public class RoutesOptionItems : IOptionItems
    {
        private readonly IAtlasRoutesService _atlasRoutesService;

        public RoutesOptionItems(IAtlasRoutesService atlasRoutesService)
        {
            _atlasRoutesService = atlasRoutesService;
        }

        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            IEnumerable<string> routes = [.. _atlasRoutesService.GetRoutes()
                .Where(s => !s.Contains("/alert/"))
                .Select(s => s.Remove(0, 1))
                .Order()];

            return [new OptionItem { Id = string.Empty, Display = string.Empty }, .. routes.Select(r => new OptionItem { Id = r, Display = r })];
        }
    }
}
