using Atlas.Blazor.Web.Interfaces;
using Atlas.Blazor.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Atlas.Blazor.Web.App.Extensions
{
    public static class AtlasServiceProviderExtensions
    {
        public static IServiceProvider AddAtlasRoutablePages(this IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetRequiredService<IAtlasRoutesService>() is AtlasRoutesService atlasRoutesService)
            {
                List<string> routes = GetRoutesToRender(typeof(_Imports).Assembly);

                atlasRoutesService.AddRoutes(routes);
            }

            return serviceProvider;
        }

        public static IServiceProvider AddAdditionalRoutableAssemblies(this IServiceProvider serviceProvider, params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                if (serviceProvider.GetRequiredService<IAtlasRoutesService>() is AtlasRoutesService atlasRoutesService)
                {
                    List<string> routes = GetRoutesToRender(assembly);

                    atlasRoutesService.AddRoutes(routes);
                }
            }

            return serviceProvider;
        }

        private static List<string> GetRoutesToRender(Assembly assembly)
        {
            IEnumerable<Type> components = assembly
                .ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(ComponentBase)));

            List<string> routes = components
                .Select(c => GetRouteFromComponent(c))
                .Where(config => config is not null)
                .ToList();

            return [.. routes.Where(s => !string.IsNullOrWhiteSpace(s))];
        }

        private static string GetRouteFromComponent(Type component)
        {
            var attributes = component.GetCustomAttributes(inherit: true);

            var routeAttribute = attributes.OfType<RouteAttribute>().FirstOrDefault();

            if(routeAttribute == null) return string.Empty;

            return routeAttribute.Template;
        }
    }
}
