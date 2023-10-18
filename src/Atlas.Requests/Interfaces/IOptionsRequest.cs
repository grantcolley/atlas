using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IOptionsRequest
    {
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItems(IEnumerable<OptionsArg> optionsArgs);
    }
}
