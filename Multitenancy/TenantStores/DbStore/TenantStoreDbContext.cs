using Microsoft.EntityFrameworkCore;
using Multitenancy.Model.Abstraction;
using Multitenancy.TenantProvider;

namespace Multitenancy.TenantStores.DbStore;

public class TenantStoreDbContext<TTenant, TKey>: DbContext
    where TTenant : class, IDbTenant<TKey>
{
    
 
    public TenantStoreDbContext()
    {
      
    }

    public DbSet<TTenant> Tenants { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}