namespace Multitenancy.Exceptions;

public class TenantExtractionException : Exception
{
    public TenantExtractionException(string message) : base(message)
    {
        
    }
}