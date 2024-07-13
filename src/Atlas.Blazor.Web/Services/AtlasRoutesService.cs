using Atlas.Blazor.Web.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Atlas.Blazor.Web.App")]

namespace Atlas.Blazor.Web.Services
{
    public class AtlasRoutesService : IAtlasRoutesService
    {
        private readonly List<string> _routes = [];

        internal void AddRoutes(IEnumerable<string> routes)
        {
            ArgumentNullException.ThrowIfNull(routes, nameof(routes));

            IEnumerable<string> duplicates = _routes.Intersect(routes);

            if (duplicates.Any()) throw new Exception($"Route(s) already exist {string.Join(" ", duplicates)}");

            _routes.AddRange(routes);
        }

        public IEnumerable<string> GetRoutes()
        {
            return _routes;
        }
    }
}
