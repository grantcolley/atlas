namespace Atlas.Blazor.Components.Interfaces
{
    public interface IAtlasCache
    {
        T? Get<T>(string key);
        bool Remove(string key);
        void Set(string key, object value);
    }
}
