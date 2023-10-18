using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IOptionsData
    {
        Task<IEnumerable<OptionItem>> GetOptionsAsync(IEnumerable<OptionsArg> args);
    }
}
