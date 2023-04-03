using Microsoft.EntityFrameworkCore;
using Multitenancy.Model;
using Multitenancy.Model.Abstraction;
using Multitenancy.TenantProvider;

namespace Multitenancy.TenantScopedServices;

public class BaseDbContext<TTenant, TKey>  : DbContext
where TTenant : IDbTenant<TKey>
{
    private readonly ITenantProvider<TTenant, TKey> TenantProvider;
    public BaseDbContext(ITenantProvider<TTenant, TKey> tenantProvider)
    {
        TenantProvider = tenantProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(TenantProvider.Tenant.SqlConnectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        base.OnModelCreating(modelBuilder);
    }
}