using Multitenancy.Model.Abstraction;
using Multitenancy.TenantExtractors;
using Multitenancy.TenantProvider;

namespace Multitenancy.MIddleware;

public class TenantExtractionMiddleware<TTenant,TKey>
    where TTenant : ITenant<TKey> 
 // where  TKey : struct, IConvertible
{
    private readonly RequestDelegate _next;

    public TenantExtractionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
   
    //TODO generic  type for Tenant can be used 
    public async Task InvokeAsync(HttpContext context, ITenantRetriever retriever, ITenantStore<TTenant, TKey> tenantStore, ITenantProvider<ITenant<TKey>, TKey> tenantProvider)
    {
        //todo extract this logic
        //it can be id or name
        var tenantIdentifier = retriever.RetrieveTenantIdentifier(context);
        TenantIdentificationType identificationType;
      
        //TODO retirve from configuration
        identificationType = TenantIdentificationType.Id;
        

        TKey tenantId;
        
        TTenant tenant;
        if (identificationType == TenantIdentificationType.Id)
        {
            tenantId = (TKey) Convert.ChangeType(tenantIdentifier, typeof(TKey));
            tenant = tenantStore.GetById(tenantId);
        }
        else
        {
            tenant = tenantStore.GetByName(tenantIdentifier);
        }

        context.Items["tenantId"] = tenant.Id;
        context.Items["tenant"] = tenant;

        await _next(context);

    }
}

public static class TenantExtractionMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantExtraction<TTenant, TKey>(this IApplicationBuilder builder)   
        where TTenant : ITenant<TKey> 
    {
        return builder.UseMiddleware<TenantExtractionMiddleware<TTenant, TKey>>();
    }
}