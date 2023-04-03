using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Multitenancy.Model;
using Multitenancy.Model.Abstraction;
using Multitenancy.TenantProvider;

namespace Multitenancy.TenantScopedServices;

public class BaseSingleDbContext<TTenant, TKey>  : DbContext
where TTenant : IDbTenant<TKey>
{
    private readonly ITenantProvider<TTenant, TKey> TenantProvider;
    private readonly string tenantIdPropertyName;
    public BaseSingleDbContext(ITenantProvider<TTenant, TKey> tenantProvider)
    {
        TenantProvider = tenantProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       // optionsBuilder.UseSqlServer(TenantProvider.Tenant.SqlConnectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected void DefineGlobalfilters(ModelBuilder modelBuilder)
    {
        var tenantId = TenantProvider.TenantId;
     
       
        //sample     Expression<Func<TTenant, bool>> filterExpr = entity => entity.TenantId == tenantId;
   
        var entityParameter = Expression.Parameter(typeof(object), "entity");

        var expBody = Expression.Equal(
            Expression.Property(entityParameter, typeof(TTenant).GetProperty(tenantIdPropertyName)),
            Expression.Variable(typeof(TKey), nameof(tenantId)));
   
        var filterExpr = (Expression<Func<object, bool>>) Expression.Lambda( expBody, entityParameter);
       

        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
        {
          
            //som types should be filtered
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(object)))
            {

                var clrType = mutableEntityType.ClrType;
                if (clrType.GetProperty(tenantIdPropertyName) == null)
                {
                    //TODO throw an exception or just skip
                }
                var parameter = Expression.Parameter(clrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

          
                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }
    
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        DefineGlobalfilters(modelBuilder);
    }


}