namespace Atlas.Requests.Interfaces
{
    public interface IGenericRequests
    {
        Task<IResponse<IEnumerable<T>>> GetGenericListAsync<T>(string endpoint) where T : class, new();
        Task<IResponse<T>> GetGenericModelAsync<T>(int id, string endpoint) where T : class, new();
    }
}
