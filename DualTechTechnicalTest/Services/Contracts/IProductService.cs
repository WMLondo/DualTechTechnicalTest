using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Services.Contracts;

public interface IProductService
{
    public Task<Result<IEnumerable<ProductDataTransferObject>>> GetAllAsync(
        CancellationToken cancellationToken = default
    );

    public Task<Result<ProductDataTransferObject>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );

    public Task<Result<ProductDataTransferObject>> CreateAsync(
        ProductDataTransferObject body,
        CancellationToken cancellationToken = default
    );

    public Task<Result<ProductDataTransferObject>> UpdateAsync(
        ProductDataTransferObject body,
        CancellationToken cancellationToken = default
    );
}
