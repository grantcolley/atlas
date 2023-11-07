using Atlas.Core.Constants;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Atlas.Core.Options
{
    public class PageCodeOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<OptionsArg> args)
        {
            IEnumerable<OptionItem>? options = typeof(PageCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
              .Where(f => f.FieldType == typeof(string))
              .Select(f => new OptionItem
              {
                  Id = (string?)f.GetValue(null),
                  Display = (string?)f.GetValue(null)
              })
              .OrderBy(oi => oi.Display)
              .ToList();

            List<OptionItem> optionItems = new();

            if(options != null)
            {
                optionItems.AddRange(options);
            }

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
