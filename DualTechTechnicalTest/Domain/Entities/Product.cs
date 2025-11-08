namespace DualTechTechnicalTest.Domain.Entities;

public sealed class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock  { get; set; }
    
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}