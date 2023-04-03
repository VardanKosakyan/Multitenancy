namespace Multitenancy.TenantExtractors.PolymorphicTenantRetriver;

public abstract class PolymorphicCondition
{
    public abstract bool Satisfy(HttpContext context);
}