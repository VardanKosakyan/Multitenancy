using Multitenancy.Exceptions;
using Multitenancy.TenantExtractors.PolymorphicTenantRetriver;
using Multitenancy.TenantProvider;

namespace Multitenancy.TenantExtractors;

public class PolymorphicTenantRetriever: ITenantRetriever
{
    public IList<PolymorphicRetrieverRule> Rules { get; set; }
    
    public PolymorphicRetrieverRule MatchedRule { get; set; }
 
    //retrives tenant, and mark Rule which where used
    public string RetrieveTenantIdentifier(HttpContext context, bool throwExceptionIfNotExists = true)
    {
        //todo consider using null whtn identidier not found
        foreach (var rule in Rules)
        {
            if (rule.Condition?.Satisfy(context) ?? true)
            {
                try
                {
                    var tenantIdentifier = rule.Retriver.RetrieveTenantIdentifier(context, false);
                    if (tenantIdentifier != null)
                    {
                        MatchedRule = rule;
                        return tenantIdentifier;
                    }
                    
                }
                catch (TenantExtractionException e)
                {
                    continue;
                }
                catch (Exception e)
                {
                    throw;
                }
               
            }
        }

        return null;
    }
}