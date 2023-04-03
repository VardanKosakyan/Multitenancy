using Multitenancy.Model;
using Multitenancy.Model.Abstraction;

namespace Multitenancy.TenantStores;

public class TenantConfigurationStore<TTenant, TKey> : ITenantPersistenceStore<TTenant, TKey>
    where TTenant : ITenant<TKey> 
    where TKey : struct
{
    public TTenant GetById(TKey id)
    {
        throw new NotImplementedException();
    }

    public TTenant GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TTenant> GetAll()
    {
        throw new NotImplementedException();
    }
}