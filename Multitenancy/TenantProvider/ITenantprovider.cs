using Multitenancy.Model.Abstraction;

namespace Multitenancy.TenantProvider;

public interface ITenantProvider<TTenant, TKey>
    where TTenant : ITenant<TKey> 
 
{
    public TKey TenantId { get; }
    public TTenant Tenant { get;}
        
}