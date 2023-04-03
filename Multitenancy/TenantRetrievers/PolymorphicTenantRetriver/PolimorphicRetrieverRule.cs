using Multitenancy.TenantProvider;

namespace Multitenancy.TenantExtractors.PolymorphicTenantRetriver;

public class PolymorphicRetrieverRule
{
    public PolymorphicCondition Condition { get; set; }
    public ITenantRetriever Retriver { get; set; }
    
    public TenantIdentificationType TenantIdentificationType { get; set; }
}