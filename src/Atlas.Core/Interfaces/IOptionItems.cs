using Atlas.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atlas.Core.Interfaces
{
    public interface IOptionItems
    {
        Task<IEnumerable<OptionItem>> GetOptionItemsAsync(IEnumerable<OptionsArg> args);
    }
}
