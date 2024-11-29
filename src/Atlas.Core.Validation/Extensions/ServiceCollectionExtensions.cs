using Atlas.Core.Models;
using Atlas.Core.Validation.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Core.Validation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAtlasValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<Module>, ModuleValidator>();
            services.AddScoped<IValidator<Category>, CategoryValidator>();
            services.AddScoped<IValidator<Page>, PageValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<Role>, RoleValidator>();
            services.AddScoped<IValidator<Permission>, PermissionValidator>();
            return services;
        }
    }
}
