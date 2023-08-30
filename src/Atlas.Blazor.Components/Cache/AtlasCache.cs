using Atlas.Blazor.Components.Interfaces;

namespace Atlas.Blazor.Components.Cache
{
    public class AtlasCache : IAtlasCache
    {
        private readonly Dictionary<string, object> cache = new();

        public T? Get<T>(string key)
        {
            if(key  == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (cache.ContainsKey(key))
            {
                return (T?)cache.GetValueOrDefault<string, object>(key);
            }
            else
            {
                return default;
            }
        }

        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (cache.ContainsKey(key))
            {
                return cache.Remove(key);
            }

            return true;
        }

        public void Set(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (cache.ContainsKey(key))
            {
                cache[key] = value;
            }
            else
            {
                cache.Add(key, value);
            }
        }
    }
}
