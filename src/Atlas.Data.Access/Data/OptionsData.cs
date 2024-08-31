using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Atlas.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Atlas.Data.Access.Data
{
    public class OptionsData : AuthorisationData<OptionsData>, IOptionsData
    {
        private readonly Dictionary<string, Func<IEnumerable<OptionsArg>, CancellationToken, Task<IEnumerable<OptionItem>>>> optionItems = [];
        private readonly Dictionary<string, Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>> genericOptionItems = [];

        public OptionsData(ApplicationDbContext applicationDbContext, ILogger<OptionsData> logger)
            : base(applicationDbContext, logger)
        {
            optionItems[Options.PERMISSIONS_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, CancellationToken, Task<IEnumerable<OptionItem>>>(GetPermissionsOptionItemsAsync);

            genericOptionItems[Options.MODULES_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>(GetModulesAsync);
            genericOptionItems[Options.CATEGORIES_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>(GetCategoriesAsync);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            string? optionsCode = optionsArgs.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if (string.IsNullOrWhiteSpace(optionsCode)) throw new AtlasException($"{nameof(optionsCode)} is null", new NullReferenceException(nameof(optionsCode)));

            if (optionItems.TryGetValue(optionsCode, out Func<IEnumerable<OptionsArg>, CancellationToken, Task<IEnumerable<OptionItem>>>? value))
            {
                try
                {
                    return await value.Invoke(optionsArgs, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    throw new AtlasException(ex.Message, ex, $"OptionsCode={optionsCode}");
                }
            }

            throw new AtlasException($"{optionsCode} not found", new NotImplementedException(optionsCode), $"OptionsCode={optionsCode}");
        }

        public async Task<string> GetGenericOptionsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            string? optionsCode = optionsArgs.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if (string.IsNullOrWhiteSpace(optionsCode)) throw new AtlasException($"{nameof(optionsCode)} is null", new NullReferenceException(nameof(optionsCode)));

            if (genericOptionItems.TryGetValue(optionsCode, out Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>? value))
            {
                try
                {
                    return await value.Invoke(optionsArgs, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new AtlasException(ex.Message, ex, $"OptionsCode={optionsCode}");
                }
            }

            throw new AtlasException($"{optionsCode} not found", new NotImplementedException(optionsCode), $"OptionsCode={optionsCode}");
        }

        private async Task<IEnumerable<OptionItem>> GetPermissionsOptionItemsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            List<Permission> permissions = await _applicationDbContext.Permissions
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (permissions.Count > 0)
            {
                return
                [
                    new OptionItem { Id = string.Empty, Display = string.Empty },
                .. permissions.Select(p => new OptionItem { Id = p.Code, Display = p.Name })
                ];
            }
            else
            {
                return [];
            }
        }

        private async Task<string> GetModulesAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            List<Module> modules = await _applicationDbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (modules.Count > 0)
            {
                modules.Insert(0, new Module { ModuleId = -1 });
            }

            return JsonSerializer.Serialize(modules);
        }

        private async Task<string> GetCategoriesAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            List<Category> categories = await _applicationDbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (categories.Count > 0)
            {
                categories.Insert(0, new Category { CategoryId = -1 });
            }

            return JsonSerializer.Serialize(categories);
        }
    }
}
