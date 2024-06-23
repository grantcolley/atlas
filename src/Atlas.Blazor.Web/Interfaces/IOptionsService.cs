using Atlas.Core.Models;

namespace Atlas.Blazor.Web.Interfaces
{
    public interface IOptionsService
    {
        bool ContainsOptions(string optionsCode);
        IEnumerable<OptionItem> GetOptionItems(string optionsCode);
        IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> optionsArgs);
    }
}
