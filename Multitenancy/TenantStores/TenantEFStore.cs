using Multitenancy.Exceptions;
using Multitenancy.Model.Abstraction;
using Multitenancy.TenantStores.DbStore;

namespace Multitenancy.TenantStores;

public class TenantEFStore<TTenant, TKey> : ITenantPersistenceStore<TTenant, TKey>
    where TTenant : class, IDbTenant<TKey> 
  //  where TKey : struct
{

    protected readonly TenantStoreDbContext<TTenant, TKey> _context;
    public  TenantEFStore(TenantStoreDbContext<TTenant, TKey> context)
    {
        _context = context;
    }
    public TTenant GetById(TKey id)
    {
        var tenant = _context.Tenants.Find(id);
        if (tenant is null)
        {
            throw new TenantNotFoundException("Tenant not found");
        }
        return tenant;
    }

    public TTenant GetByName(string name)
    {
        var tenant =  _context.Tenants.FirstOrDefault(t => t.Name == name);
        if (tenant is null)
        {
            throw new TenantNotFoundException("Tenant not found");
        }
        return tenant;
    }

    public IEnumerable<TTenant> GetAll()
    {
        return _context.Tenants.ToList();
    }
}
