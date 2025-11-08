namespace DualTechTechnicalTest.Domain;

public sealed class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public ICollection<Order>? Orders { get; set; }
}