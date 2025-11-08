namespace DualTechTechnicalTest.Domain.Entities;

public sealed class OrderDetail
{
    public int Id { get; set; }

    public decimal Quantity { get; set; }

    public decimal Tax  { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public int OrderId { get; set; }

    public Order? Order { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }
    
}