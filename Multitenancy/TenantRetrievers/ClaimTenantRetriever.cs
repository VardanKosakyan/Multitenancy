using Multitenancy.Exceptions;
using Multitenancy.TenantProvider;

namespace Multitenancy.TenantExtractors;

public class ClaimTenantRetriever : ITenantRetriever
{
    public string ClaimName { get; set; }
    public string RetrieveTenantIdentifier(HttpContext context, bool throwExceptionIfNotExists = true)
    {
        //TODO concider using identities
        var claims = context.User.Claims;
        var claim = claims.FirstOrDefault(c => c.Type == ClaimName);
        if (claim == null && throwExceptionIfNotExists)
        {
            throw new TenantExtractionException($"Claim with provided name {ClaimName} does not exists");
        }

        return claim?.Value;
    }
}