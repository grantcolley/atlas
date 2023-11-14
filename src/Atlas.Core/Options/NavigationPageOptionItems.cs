using Atlas.Core.Attributes;
using Atlas.Core.Dynamic;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlas.Core.Options
{
    public class NavigationPageOptionItems : IOptionItems
    {
        public Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<OptionsArg> args)
        {
            var containers = TypeAttributeHelper.GetHeadwayTypesByAttribute(typeof(NavigationPageAttribute));

            List<OptionItem> optionItems = new();

            optionItems.AddRange((from c in containers
                                  orderby c.Name
                                  select new OptionItem
                                  {
                                      Id = c.Namespace,
                                      Display = c.DisplayName
                                  }).ToList());

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
