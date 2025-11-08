namespace DualTechTechnicalTest.Domain.Entities;

public sealed class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Identity { get; set; }
    
    public ICollection<Order>? Orders { get; set; }
}