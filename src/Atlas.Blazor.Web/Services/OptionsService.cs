using Atlas.Blazor.Web.Interfaces;
using Atlas.Blazor.Web.OptionItems;
using Atlas.Core.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using Atlas.Core.Options;

namespace Atlas.Blazor.Web.Services
{
    public class OptionsService : IOptionsService
    {
        private readonly Dictionary<string, IOptionItems> _options = [];

        public OptionsService()
        {
            _options.Add(Options.PAGE_CODES, new PageCodeOptionItems());
            _options.Add(Options.NAVIGATION_PAGES, new NavigationPageOptionItems());
            _options.Add(Options.ICON_SIZE20_OPTION_ITEMS, new IconSize20OptionItems());
        }

        public bool ContainsOptions(string optionsCode)
        {
            return _options.ContainsKey(optionsCode); 
        }
    
        public IEnumerable<OptionItem> GetOptionItems(string optionsCode)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(optionsCode);

            List<OptionsArg> options = [new() { Name = Options.OPTIONS_CODE, Value = optionsCode }];
            return GetOptionItems(options);
        }

        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> optionsArgs)
        {
            ArgumentNullException.ThrowIfNull(optionsArgs);

            string? optionsCode = optionsArgs.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if (optionsCode == null) throw new NullReferenceException(nameof(optionsCode));

            if (!_options.TryGetValue(optionsCode, out IOptionItems? value)) throw new KeyNotFoundException(nameof(optionsCode));

            return value.GetOptionItems(optionsArgs);
        }
    }
}
