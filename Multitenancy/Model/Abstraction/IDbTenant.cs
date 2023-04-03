namespace Multitenancy.Model.Abstraction;

public interface IDbTenant<TKey> : ITenant<TKey>
{
    public TKey Id { get; set; }
    public string? Name { get; set; }
    public DatabaseType DatabaseType { get; set; }
    public string SqlConnectionString { get; set; }
}