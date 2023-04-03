using Microsoft.AspNetCore.Http.Extensions;
using Multitenancy.Exceptions;

namespace Multitenancy.TenantExtractors;

public class DomainTenantRetriever : ITenantRetriever
{
    protected readonly int depth = 2;
    public string RetrieveTenantIdentifier(HttpContext context, bool throwExceptionIfNotExists = true)
    {
        string domainName = context.Request.Host.Host;
        var segments = domainName.Split('.');
        if (segments.Count() < depth + 1)
        {
            if (throwExceptionIfNotExists)
            {
                throw new TenantExtractionException($"Provided dept:{depth} is more that segments ni the domain name");
            }
           
            return null;
        }

        return segments[^depth];
    }
}