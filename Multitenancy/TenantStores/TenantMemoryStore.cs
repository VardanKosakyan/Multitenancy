using System.Data;
using Microsoft.Extensions.Caching.Memory;
using Multitenancy.Exceptions;
using Multitenancy.Model.Abstraction;

namespace Multitenancy.TenantStores;

public class TenantMemoryStore<TTenant, TKey> : ITenantStore<TTenant, TKey>
    where TTenant : ITenant<TKey>
{
    protected readonly ITenantPersistenceStore<TTenant, TKey> PersistenceStore;
    protected readonly IMemoryCache MemoryCache;
    protected readonly string Prefix;
    protected readonly bool StoreByName = true;
    private const string Indicator = "indicator";
    private readonly MemoryCacheEntryOptions CacheEntryOptions;

    //TODO investigrate injection of persiosnace and memore store
    public TenantMemoryStore(ITenantPersistenceStore<TTenant, TKey> persistenceStore, IMemoryCache memoryCache)
    {
        PersistenceStore = persistenceStore;
        MemoryCache = memoryCache;
        Prefix = "Tenant.";
        //TODO get from config
        CacheEntryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(1),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

    }

    private string getCacheIdKey(TKey id) => Prefix + ":" + "id:" + id?.ToString();
    private string getCacheNameKey(string name) => Prefix + ":" + "name:" + name;

    private string getIndicatorKey() => Prefix + ":" + Indicator;

    private void UpdateCache()
    {
        var tenants = PersistenceStore.GetAll();
        foreach (var tenant in tenants)
        {
            MemoryCache.Set(getCacheIdKey(tenant.Id), tenant, CacheEntryOptions);
            if (StoreByName)
            {
                if (string.IsNullOrEmpty(tenant.Name))
                {
                    throw new NoNullAllowedException("Tenant name is null");
                }
                MemoryCache.Set(getCacheNameKey(tenant.Name), tenant, CacheEntryOptions);
            }
            MemoryCache.Set(getIndicatorKey(), tenants.Count().ToString());
        }
    }

    public TTenant GetById(TKey id)
    {
        if (MemoryCache.TryGetValue(getIndicatorKey(), out _))
        {
            UpdateCache();
        }

        var tenant =  MemoryCache.GetOrCreate<TTenant>(getCacheIdKey(id), cacheEntry =>
        {
            //TODO make expiration configurabe
            cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return PersistenceStore.GetById(id);
        });
        
        if (tenant == null)
        {
            throw new TenantNotFoundException("Tenant not found");
        }
        return tenant;
    }

    public TTenant GetByName(string name)
    {
        if (!StoreByName)
        {
            throw new InvalidOperationException("Store tenants by name is not supporting");
        }
        
        if (MemoryCache.TryGetValue(getIndicatorKey(), out _))
        {
            UpdateCache();
        }

        var tenant =  MemoryCache.GetOrCreate<TTenant>(getCacheNameKey(name), cacheEntry =>
        {
            //TODO make expiration configurabe
            cacheEntry.SlidingExpiration = CacheEntryOptions.SlidingExpiration;
            cacheEntry.AbsoluteExpirationRelativeToNow = CacheEntryOptions.AbsoluteExpirationRelativeToNow;
            return PersistenceStore.GetByName(name);
        });
        if (tenant == null)
        {
            throw new TenantNotFoundException("Tenant not found");
        }
        return tenant;

    }

    public IEnumerable<TTenant> GetAll()
    {
        if (MemoryCache.TryGetValue(Prefix + "indicator", out _))
        {
            UpdateCache();
        }
        return PersistenceStore.GetAll();
    }
}