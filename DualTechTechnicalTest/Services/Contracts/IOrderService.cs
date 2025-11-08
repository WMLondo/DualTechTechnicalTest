using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Services.Contracts;

public interface IOrderService
{
    public Task<Result<OrderDataTransferObject>> CreateAsync(CreateOrderDataTransferObject body,
        CancellationToken cancellationToken = default);
}