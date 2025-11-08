using AutoMapper;
using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;
using DualTechTechnicalTest.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DualTechTechnicalTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await clientService.GetAllAsync(cancellationToken);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await clientService.GetByIdAsync(id, cancellationToken);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ClientDataTransferObject body,
        CancellationToken cancellationToken
    )
    {
        var result = await clientService.CreateAsync(body, cancellationToken);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] ClientDataTransferObject body,
        CancellationToken cancellationToken
    )
    {
        var result = await clientService.UpdateAsync(body, cancellationToken);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
