namespace Multitenancy.TenantExtractors;

public interface ITenantRetriever
{
    //TODO consider type of key name or id
    public string RetrieveTenantIdentifier(HttpContext context, bool throwExceptionIfNotExists = true );
}