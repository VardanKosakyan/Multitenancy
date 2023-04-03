using Multitenancy.Model.Abstraction;
using Multitenancy.TenantExtractors;

namespace Multitenancy.TenantProvider;

public class TenantProvider<TTenant,TKey>: ITenantProvider<TTenant,TKey>
    where TTenant : class, ITenant<TKey> 
    where TKey : struct
{

    protected readonly ITenantStore<TTenant, TKey> TenantStore;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    
    public TenantProvider(ITenantStore<TTenant, TKey> tenantStore, IHttpContextAccessor httpContextAccessor)
    {
        TenantStore = tenantStore;
        HttpContextAccessor = httpContextAccessor;
    }

    public TKey TenantId => (TKey)HttpContextAccessor.HttpContext.Items["tenantId"];
    public TTenant Tenant => (TTenant)HttpContextAccessor.HttpContext.Items["tenant"];

}