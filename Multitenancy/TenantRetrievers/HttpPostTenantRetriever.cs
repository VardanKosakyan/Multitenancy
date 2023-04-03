namespace Multitenancy.TenantExtractors;

public class HttpPostTenantRetriever : ITenantRetriever
{
    public string RetrieveTenantIdentifier(HttpContext context, bool throwExceptionIfNotExists = true)
    {
        throw new NotImplementedException();
    }
}