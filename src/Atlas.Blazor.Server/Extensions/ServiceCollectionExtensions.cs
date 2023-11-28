using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Atlas.Blazor.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Additional assemblies to eager load into memory at runtime.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="assemblies">A collection of assemblies to be eager loaded at startup.</param>
        /// <returns>The services collection.</returns>
        public static IServiceCollection EagerLoadAdditionalAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            // Intentionally returns services without actually doing anything.
            return services;
        }
    }
}
