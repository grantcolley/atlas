using Atlas.Core.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Atlas.Data.Access.Data
{
    public class OptionsData : AuthorisationData<OptionsData>, IOptionsData
    {
        private readonly Dictionary<string, Func<IEnumerable<OptionsArg>, CancellationToken, Task<IEnumerable<OptionItem>>>> optionItems = new();
        private readonly Dictionary<string, Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>> genericOptionItems = new();

        public OptionsData(ApplicationDbContext applicationDbContext, ILogger<OptionsData> logger)
            : base(applicationDbContext, logger)
        {
            optionItems[Options.PERMISSIONS_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, CancellationToken, Task<IEnumerable<OptionItem>>>(GetPermissionsOptionItemsAsync);

            genericOptionItems[Options.MODULES_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>(GetModulesAsync);
            genericOptionItems[Options.CATEGORIES_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, CancellationToken, Task<string>>(GetCategoriesAsync);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            var optionsCode = optionsArgs.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if(!string.IsNullOrWhiteSpace(optionsCode))
            {
                if (optionItems.ContainsKey(optionsCode))
                {
                    return await optionItems[optionsCode].Invoke(optionsArgs, cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            throw new NotImplementedException(optionsCode);
        }

        public async Task<string> GetGenericOptionsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            var optionsCode = optionsArgs.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if (!string.IsNullOrWhiteSpace(optionsCode))
            {
                if (genericOptionItems.ContainsKey(optionsCode))
                {
                    return await genericOptionItems[optionsCode].Invoke(optionsArgs, cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            throw new NotImplementedException(optionsCode);
        }

        private async Task<IEnumerable<OptionItem>> GetPermissionsOptionItemsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            var configs = await _applicationDbContext.Permissions
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            List<OptionItem> optionItems = new() { new OptionItem() { Id = string.Empty, Display = string.Empty } };

            optionItems.AddRange(configs.Select(p => new OptionItem { Id = p.Code, Display = p.Name }).ToList());

            return optionItems;
        }

        private async Task<string> GetModulesAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            List<Module> modules = await _applicationDbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (modules.Any())
            {
                return JsonSerializer.Serialize(modules);
            }
            else
            {
                return JsonSerializer.Serialize(new List<Module>());
            }
        }

        private async Task<string> GetCategoriesAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken)
        {
            List<Category> categories = await _applicationDbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (categories.Any())
            {
                return JsonSerializer.Serialize(categories);
            }
            else
            {
                return JsonSerializer.Serialize(new List<Category>());
            }
        }
    }
}
