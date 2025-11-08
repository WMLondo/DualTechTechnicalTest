namespace DualTechTechnicalTest.Domain.Models.DataTransferObject;

public record OrderDataTransferObject(
    int Id,
    int ClientId,
    decimal Tax,
    decimal Subtotal,
    decimal Total,
    IEnumerable<OrderDetailDataTransferObject> Details
);

public record CreateOrderDataTransferObject(
    int Id,
    int ClientId,
    IEnumerable<CreateOrderDetailDataTransferObject> Details
);
