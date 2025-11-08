namespace DualTechTechnicalTest.Domain.Entities;

public sealed class Order
{
    public int Id { get; set; }

    public decimal Tax { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public int ClientId { get; set; }

    public Client? Client { get; set; }
    
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}