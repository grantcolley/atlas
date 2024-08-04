namespace Atlas.Requests.Interfaces
{
    public interface IGenericRequests
    {
        Task<IAuthResponse<IEnumerable<T>>> GetListAsync<T>(string args, string endpoint) where T : class, new();
        Task<IAuthResponse<IEnumerable<T>>> GetListAsync<T>(string endpoint) where T : class, new();
        Task<IResponse<T>> GetModelAsync<T>(int id, string endpoint) where T : class, new();
        Task<IResponse<T>> CreateModelAsync<T>(T model, string endpoint) where T : class, new();
        Task<IResponse<T>> UpdateModelAsync<T>(T model, string endpoint) where T : class, new();
        Task<IResponse<int>> DeleteModelAsync(int id, string endpoint);
    }
}
