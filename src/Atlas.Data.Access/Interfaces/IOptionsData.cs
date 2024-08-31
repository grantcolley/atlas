using Atlas.Core.Models;
using Atlas.Data.Context;

namespace Atlas.Data.Access.Interfaces
{
    public interface IOptionsData : IAuthorisationData
    {
        Task<IEnumerable<OptionItem>> GetOptionsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken);
        Task<string> GetGenericOptionsAsync(IEnumerable<OptionsArg> optionsArgs, CancellationToken cancellationToken);
    }
}
