namespace Multitenancy.TenantExtractors.PolymorphicTenantRetriver;

public class PreviewsTenantIdentifierNotFound : PolymorphicCondition
{
    public override bool Satisfy(HttpContext context)
    {
        throw new NotImplementedException();
    }
}