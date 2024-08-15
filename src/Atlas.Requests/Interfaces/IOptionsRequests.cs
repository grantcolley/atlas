using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IOptionsRequests
    {
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItemsAsync(string optionsCode);
        Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItemsAsync(IEnumerable<OptionsArg> optionsArgs);
        Task<IResponse<IEnumerable<T>?>> GetGenericOptionsAsync<T>(string optionsCode);
        Task<IResponse<IEnumerable<T>?>> GetGenericOptionsAsync<T>(IEnumerable<OptionsArg> optionsArgs);
    }
}
