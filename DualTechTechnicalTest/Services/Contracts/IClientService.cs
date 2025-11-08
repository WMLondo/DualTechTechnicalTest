using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;

namespace DualTechTechnicalTest.Services.Contracts;

public interface IClientService
{
    public Task<Result<IEnumerable<ClientDataTransferObject>>> GetAllAsync(
        CancellationToken cancellationToken = default
    );

    public Task<Result<ClientDataTransferObject>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<Result<ClientDataTransferObject>> CreateAsync(
        ClientDataTransferObject body,
        CancellationToken cancellationToken = default
    );

    public Task<Result<ClientDataTransferObject>> UpdateAsync(
        ClientDataTransferObject body,
        CancellationToken cancellationToken = default
    );
}
