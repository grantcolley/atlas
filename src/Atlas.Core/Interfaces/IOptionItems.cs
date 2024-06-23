using Atlas.Core.Models;
using System.Collections.Generic;

namespace Atlas.Core.Interfaces
{
    public interface IOptionItems
    {
        IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args);
    }
}
