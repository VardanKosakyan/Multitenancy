namespace Multitenancy.Model.Abstraction;

public interface ITenant<TKey>
{
    //main tenant identifier
    TKey Id { get; set; }
    string? Name { get; set; }
}