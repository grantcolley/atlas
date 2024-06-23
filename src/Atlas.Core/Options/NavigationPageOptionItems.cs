using Atlas.Core.Attributes;
using Atlas.Core.Dynamic;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Core.Options
{
    public class NavigationPageOptionItems : IOptionItems
    {
        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            var containers = TypeAttributeHelper.GetAtlasTypesByAttribute(typeof(NavigationPageAttribute));

            List<OptionItem> optionItems = [];

            optionItems.AddRange((from c in containers
                                  orderby c.Name
                                  select new OptionItem
                                  {
                                      Id = c.Namespace,
                                      Display = c.DisplayName
                                  }).ToList());

            return optionItems.AsEnumerable();
        }
    }
}
