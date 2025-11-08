using AutoMapper;
using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;
using Microsoft.AspNetCore.Mvc;

namespace DualTechTechnicalTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var clients = await unitOfWork.ClientRepository.GetAllAsync(
            cancellationToken: cancellationToken
        );

        var clientsDto = mapper.Map<IEnumerable<ClientDataTransferObject>>(clients);

        return Ok(
            Result<IEnumerable<ClientDataTransferObject>>.SuccessResponse(
                clientsDto,
                "Clients retrieved successfully"
            )
        );
    }

    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken: cancellationToken
        );

        if (client is null)
        {
            return NotFound(
                Result<ClientDataTransferObject>.FailureResponse($"Client with ID {id} not found")
            );
        }

        var clientDto = mapper.Map<ClientDataTransferObject>(client);

        return Ok(
            Result<ClientDataTransferObject>.SuccessResponse(
                clientDto,
                "Client retrieved successfully"
            )
        );
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ClientDataTransferObject body,
        CancellationToken cancellationToken
    )
    {
        if (body is not { Id: 0 })
        {
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse(
                    "Invalid ID value for creation - ID should be 0"
                )
            );
        }

        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Identity == body.Identity,
            cancellationToken: cancellationToken
        );

        if (client is not null)
        {
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse(
                    "A client with this identity already exists"
                )
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
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse("Failed to create client.")
            );
        }

        var clientDto = mapper.Map<ClientDataTransferObject>(createdClient);

        return Ok(
            Result<ClientDataTransferObject>.SuccessResponse(
                clientDto,
                "Client created successfully"
            )
        );
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] ClientDataTransferObject body,
        CancellationToken cancellationToken
    )
    {
        if (body is { Id: 0 })
        {
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse(
                    "Invalid ID value for update - ID cannot be 0"
                )
            );
        }

        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Identity == body.Identity,
            cancellationToken: cancellationToken
        );

        if (client is not null && client.Id != body.Id)
        {
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse(
                    "A client with this identity already exists"
                )
            );
        }

        var toUpdateClient = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Id == body.Id,
            cancellationToken: cancellationToken
        );

        if (toUpdateClient is null)
        {
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse("Client to update not found")
            );
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
            return BadRequest(
                Result<ClientDataTransferObject>.FailureResponse("Failed to update client.")
            );
        }

        var clientDto = mapper.Map<ClientDataTransferObject>(updatedClient);

        return Ok(
            Result<ClientDataTransferObject>.SuccessResponse(
                clientDto,
                "Client updated successfully"
            )
        );
    }
}
