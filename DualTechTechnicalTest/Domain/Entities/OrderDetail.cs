namespace DualTechTechnicalTest.Domain;

public class OrderDetail
{
    public int Id { get; set; }

    public decimal Quantity { get; set; }

    public decimal Tax  { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }
    
}