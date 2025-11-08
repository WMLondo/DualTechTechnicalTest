using AutoMapper;
using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;
using DualTechTechnicalTest.Services.Contracts;

namespace DualTechTechnicalTest.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    public async Task<Result<IEnumerable<ProductDataTransferObject>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var products = await unitOfWork.ProductRepository.GetAllAsync(
            cancellationToken: cancellationToken
        );

        var productsDto = mapper.Map<IEnumerable<ProductDataTransferObject>>(products);

        return Result<IEnumerable<ProductDataTransferObject>>.SuccessResponse(
            productsDto,
            "Products retrieved successfully"
        );
    }

    public async Task<Result<ProductDataTransferObject>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var product = await unitOfWork.ProductRepository.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken: cancellationToken
        );

        if (product is null)
        {
            return Result<ProductDataTransferObject>.FailureResponse(
                $"Product with ID {id} not found"
            );
        }

        var productDto = mapper.Map<ProductDataTransferObject>(product);

        return Result<ProductDataTransferObject>.SuccessResponse(
            productDto,
            "Product retrieved successfully"
        );
    }

    public async Task<Result<ProductDataTransferObject>> CreateAsync(
        ProductDataTransferObject body,
        CancellationToken cancellationToken = default
    )
    {
        if (body is not { Id: 0 })
        {
            Result<ProductDataTransferObject>.FailureResponse(
                "Invalid ID value for creation - ID should be 0"
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
            return Result<ProductDataTransferObject>.FailureResponse("Failed to create product.");
        }

        var productDto = mapper.Map<ProductDataTransferObject>(createdProduct);

        return Result<ProductDataTransferObject>.SuccessResponse(
            productDto,
            "Product created successfully"
        );
    }

    public async Task<Result<ProductDataTransferObject>> UpdateAsync(
        ProductDataTransferObject body,
        CancellationToken cancellationToken = default
    )
    {
        if (body is { Id: 0 })
        {
            return Result<ProductDataTransferObject>.FailureResponse(
                "Invalid ID value for update - ID cannot be 0"
            );
        }

        var toUpdateProduct = await unitOfWork.ProductRepository.FirstOrDefaultAsync(
            x => x.Id == body.Id,
            cancellationToken: cancellationToken
        );

        if (toUpdateProduct is null)
        {
            return Result<ProductDataTransferObject>.FailureResponse("Product to update not found");
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
            return Result<ProductDataTransferObject>.FailureResponse("Failed to update product.");
        }

        var productDto = mapper.Map<ProductDataTransferObject>(updatedProduct);

        return Result<ProductDataTransferObject>.SuccessResponse(
            productDto,
            "Product updated successfully"
        );
    }
}
