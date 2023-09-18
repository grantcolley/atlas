namespace Atlas.Blazor.Shared.Interfaces
{
    public interface IAtlasCache
    {
        T? Get<T>(string key);
        bool Remove(string key);
        void Set(string key, object value);
    }
}
