namespace Multitenancy.Model.Abstraction;

public interface ITenantStore<out TTenant, TKey> 
    where TTenant : ITenant<TKey> 
 //   where TKey : struct
{
    TTenant GetById(TKey id);
    TTenant GetByName(string name);
    IEnumerable<TTenant> GetAll();
}

public interface ITenantPersistenceStore<out TTenant, TKey> : ITenantStore<TTenant, TKey>
    where TTenant : ITenant<TKey>
{
    
}