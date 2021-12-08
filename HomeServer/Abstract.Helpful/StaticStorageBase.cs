using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public abstract class StaticStorageBase<TId, TStaticData>
    {
        protected readonly ConcurrentDictionary<TId, TStaticData> Cache = new();
        
        public bool TryGetFromCache(TId id, out TStaticData staticData)
        {
            lock (Cache)
            {
                if (Cache.ContainsKey(id))
                {
                    staticData = Cache[id];
                    return true;
                }
                else
                {
                    staticData = default;
                    return false;
                }
            }
        }
        
        public async Task<TStaticData> GetById(TId id)
        {
            lock (Cache)
            {
                if (Cache.ContainsKey(id))
                    return Cache[id];
            }
            var staticData = await LoadStaticData(id);
            UpdateCache(staticData);
            return staticData;
        }
        
        protected void UpdateCache(TStaticData staticData)
        {
            lock (Cache)
            {
                var id = GetId(staticData);
                if (!Cache.ContainsKey(id)) 
                    Cache.TryAdd(id, staticData);
            }
        } 
        
        protected void UpdateCache(IEnumerable<TStaticData> staticDataList)
        {
            if (staticDataList == null)
                return;
            
            lock (Cache)
            {
                foreach (var staticData in staticDataList)
                {
                    var id = GetId(staticData);
                    if (!Cache.ContainsKey(id)) 
                        Cache.TryAdd(id, staticData);
                }
            }
        } 

        protected abstract Task<TStaticData> LoadStaticData(TId id);
        protected abstract TId GetId(TStaticData staticData);
    }
}