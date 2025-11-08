using AutoMapper;
using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;
using DualTechTechnicalTest.Services.Contracts;

namespace DualTechTechnicalTest.Services;

public class ClientService(IUnitOfWork unitOfWork, IMapper mapper) : IClientService
{
    public async Task<Result<IEnumerable<ClientDataTransferObject>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var clients = await unitOfWork.ClientRepository.GetAllAsync(
            cancellationToken: cancellationToken
        );

        var clientsDto = mapper.Map<IEnumerable<ClientDataTransferObject>>(clients);

        return Result<IEnumerable<ClientDataTransferObject>>.SuccessResponse(
            clientsDto,
            "Clients retrieved successfully"
        );
    }

    public async Task<Result<ClientDataTransferObject>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken: cancellationToken
        );

        if (client is null)
        {
            return Result<ClientDataTransferObject>.FailureResponse(
                $"Client with ID {id} not found"
            );
        }

        var clientDto = mapper.Map<ClientDataTransferObject>(client);

        return Result<ClientDataTransferObject>.SuccessResponse(
            clientDto,
            "Client retrieved successfully"
        );
    }

    public async Task<Result<ClientDataTransferObject>> CreateAsync(
        ClientDataTransferObject body,
        CancellationToken cancellationToken = default
    )
    {
        if (body is not { Id: 0 })
        {
            return Result<ClientDataTransferObject>.FailureResponse(
                "Invalid ID value for creation - ID should be 0"
            );
        }

        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Identity == body.Identity,
            cancellationToken: cancellationToken
        );

        if (client is not null)
        {
            return Result<ClientDataTransferObject>.FailureResponse(
                "A client with this identity already exists"
            );
        }

        var newClient = mapper.Map<Client>(body);

        var createdClient = await unitOfWork.ClientRepository.CreateAsync(
            newClient,
            cancellationToken
        );

        var result = await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result<ClientDataTransferObject>.FailureResponse("Failed to create client.");
        }

        var clientDto = mapper.Map<ClientDataTransferObject>(createdClient);

        return Result<ClientDataTransferObject>.SuccessResponse(
            clientDto,
            "Client created successfully"
        );
    }

    public async Task<Result<ClientDataTransferObject>> UpdateAsync(
        ClientDataTransferObject body,
        CancellationToken cancellationToken = default
    )
    {
        if (body is { Id: 0 })
        {
            return Result<ClientDataTransferObject>.FailureResponse(
                "Invalid ID value for update - ID cannot be 0"
            );
        }

        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Identity == body.Identity,
            cancellationToken: cancellationToken
        );

        if (client is not null && client.Id != body.Id)
        {
            return Result<ClientDataTransferObject>.FailureResponse(
                "A client with this identity already exists"
            );
        }

        var toUpdateClient = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Id == body.Id,
            cancellationToken: cancellationToken
        );

        if (toUpdateClient is null)
        {
            return Result<ClientDataTransferObject>.FailureResponse("Client to update not found");
        }

        toUpdateClient.Name = body.Name;
        toUpdateClient.Identity = body.Identity;

        var updatedClient = await unitOfWork.ClientRepository.UpdateAsync(
            toUpdateClient,
            cancellationToken
        );

        var result = await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result<ClientDataTransferObject>.FailureResponse("Failed to update client.");
        }

        var clientDto = mapper.Map<ClientDataTransferObject>(updatedClient);

        return Result<ClientDataTransferObject>.SuccessResponse(
            clientDto,
            "Client updated successfully"
        );
    }
}
