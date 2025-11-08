using AutoMapper;
using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;
using Microsoft.AspNetCore.Mvc;

namespace DualTechTechnicalTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.GetAllAsync(
            cancellationToken: cancellationToken
        );

        var productsDto = mapper.Map<IEnumerable<ProductDataTransferObject>>(products);

        return Ok(
            Result<IEnumerable<ProductDataTransferObject>>.SuccessResponse(
                productsDto,
                "Products retrieved successfully"
            )
        );
    }

    [HttpGet("getById/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken: cancellationToken
        );

        if (product is null)
        {
            return NotFound(
                Result<ProductDataTransferObject>.FailureResponse($"Product with ID {id} not found")
            );
        }

        var productDto = mapper.Map<ProductDataTransferObject>(product);

        return Ok(
            Result<ProductDataTransferObject>.SuccessResponse(
                productDto,
                "Product retrieved successfully"
            )
        );
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ProductDataTransferObject body,
        CancellationToken cancellationToken
    )
    {
        if (body is not { Id: 0 })
        {
            return BadRequest(
                Result<ProductDataTransferObject>.FailureResponse(
                    "Invalid ID value for creation - ID should be 0"
                )
            );
        }

        var newProduct = mapper.Map<Product>(body);

        var createdProduct = await unitOfWork.ProductRepository.CreateAsync(
            newProduct,
            cancellationToken
        );

        var result = await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return BadRequest(
                Result<ProductDataTransferObject>.FailureResponse("Failed to create product.")
            );
        }

        var productDto = mapper.Map<ProductDataTransferObject>(createdProduct);

        return Ok(
            Result<ProductDataTransferObject>.SuccessResponse(
                productDto,
                "Product created successfully"
            )
        );
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] ProductDataTransferObject body,
        CancellationToken cancellationToken
    )
    {
        if (body is { Id: 0 })
        {
            return BadRequest(
                Result<ProductDataTransferObject>.FailureResponse(
                    "Invalid ID value for update - ID cannot be 0"
                )
            );
        }
        

        var toUpdateProduct = await unitOfWork.ProductRepository.FirstOrDefaultAsync(
            x => x.Id == body.Id,
            cancellationToken: cancellationToken
        );

        if (toUpdateProduct is null)
        {
            return BadRequest(
                Result<ProductDataTransferObject>.FailureResponse("Product to update not found")
            );
        }

        toUpdateProduct.Name = body.Name;
        toUpdateProduct.Description = body.Description;
        toUpdateProduct.Price = body.Price;
        toUpdateProduct.Stock = body.Stock;

        var updatedProduct = await unitOfWork.ProductRepository.UpdateAsync(
            toUpdateProduct,
            cancellationToken
        );

        var result = await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return BadRequest(
                Result<ProductDataTransferObject>.FailureResponse("Failed to update product.")
            );
        }

        var productDto = mapper.Map<ProductDataTransferObject>(updatedProduct);

        return Ok(
            Result<ProductDataTransferObject>.SuccessResponse(
                productDto,
                "Product updated successfully"
            )
        );
    }
}
