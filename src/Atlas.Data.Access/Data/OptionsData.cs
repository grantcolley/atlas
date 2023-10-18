using Atlas.Core.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class OptionsData : AuthorisationData<WeatherForecastData>, IOptionsData
    {
        private readonly Dictionary<string, Func<IEnumerable<OptionsArg>, Task<IEnumerable<OptionItem>>>> optionItems = new();

        public OptionsData(ApplicationDbContext applicationDbContext, ILogger<WeatherForecastData> logger)
            : base(applicationDbContext, logger)
        {
            optionItems[Options.PERMISSIONS_OPTION_ITEMS] = new Func<IEnumerable<OptionsArg>, Task<IEnumerable<OptionItem>>>(GetPermissionsOptionItems);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionsAsync(IEnumerable<OptionsArg> args)
        {
            var optionsCode = args.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if(!string.IsNullOrWhiteSpace(optionsCode))
            {
                if (optionItems.ContainsKey(optionsCode))
                {
                    return await optionItems[optionsCode].Invoke(args).ConfigureAwait(false);
                }
            }

            throw new NotImplementedException(optionsCode);
        }

        private async Task<IEnumerable<OptionItem>> GetPermissionsOptionItems(IEnumerable<OptionsArg> args)
        {
            var configs = await _applicationDbContext.Permissions
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            List<OptionItem> optionItems = new() { new OptionItem() };

            optionItems.AddRange(configs.Select(p => new OptionItem { Id = p.Name, Display = p.Name }).ToList());

            return optionItems;
        }
    }
}
