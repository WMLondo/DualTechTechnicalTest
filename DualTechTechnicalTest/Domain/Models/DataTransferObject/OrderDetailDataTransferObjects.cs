namespace DualTechTechnicalTest.Domain.Models.DataTransferObject;

public record OrderDetailDataTransferObject(
    int Id,
    int OrderId,
    int ProductId,
    int Quantity,
    decimal Tax,
    decimal Subtotal,
    decimal Total
);

public record CreateOrderDetailDataTransferObject(int ProductId, int Quantity);
