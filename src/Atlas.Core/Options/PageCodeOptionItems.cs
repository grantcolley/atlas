using Atlas.Core.Constants;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atlas.Core.Options
{
    public class PageCodeOptionItems : IOptionItems
    {
        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            IEnumerable<OptionItem> optionsItems = typeof(PageCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
              .Where(f => f.FieldType == typeof(string))
              .Select(f => new OptionItem
              {
                  Id = (string?)f.GetValue(null),
                  Display = (string?)f.GetValue(null)
              })
              .OrderBy(oi => oi.Display);

            return optionsItems;
        }
    }
}
