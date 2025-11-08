namespace DualTechTechnicalTest.Domain.Models.DataTransferObject;

public record ProductDataTransferObject(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock
);
