using Multitenancy.Model.Abstraction;

namespace Multitenancy.Model;

public class Tenant<TKey> : IDbTenant<TKey>
{
   public TKey Id { get; set; }
   public string Name { get; set; }
   public string UrlName { get; set; }
   //connection 
   public DatabaseType DatabaseType { get; set; }
   public string SqlConnectionString { get; set; }
   public StorageType StorageType { get; set; }
   public string StorageConnectionString { get; set; }
   
   //security
   public int PermissionGroup { get; set; }
   public string ConfigurationScope { get; set; }
   //service Type 
   
}