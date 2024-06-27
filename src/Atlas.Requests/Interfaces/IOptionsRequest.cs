using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IOptionsRequest
    {
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItemsAsync(string optionsCode);
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItemsAsync(IEnumerable<OptionsArg> optionsArgs);
        Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(IEnumerable<OptionsArg> optionsArgs);
    }
}
