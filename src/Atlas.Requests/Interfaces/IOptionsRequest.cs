using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IOptionsRequest
    {
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItems(string optionsCode);
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItems(IEnumerable<OptionsArg> optionsArgs);
        Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(IEnumerable<OptionsArg> optionsArgs);
    }
}
