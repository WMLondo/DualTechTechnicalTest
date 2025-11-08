using AutoMapper;
using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Models;
using DualTechTechnicalTest.Domain.Models.DataTransferObject;
using DualTechTechnicalTest.Services.Contracts;

namespace DualTechTechnicalTest.Services;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
{
    public async Task<Result<OrderDataTransferObject>> CreateAsync(
        CreateOrderDataTransferObject body,
        CancellationToken cancellationToken = default
    )
    {
        if (body is not { Id: 0 })
        {
            return Result<OrderDataTransferObject>.FailureResponse(
                "Invalid order ID. For order creation, ID must be 0."
            );
        }

        if (body.ClientId <= 0)
        {
            return Result<OrderDataTransferObject>.FailureResponse(
                "Invalid ClientId. Client ID must be greater than 0."
            );
        }

        var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(
            x => x.Id == body.ClientId,
            cancellationToken: cancellationToken
        );

        if (client is null)
        {
            return Result<OrderDataTransferObject>.FailureResponse(
                $"Client with ID {body.ClientId} not found. Please provide a valid client ID."
            );
        }

        if (!body.Details.Any())
        {
            return Result<OrderDataTransferObject>.FailureResponse(
                "Order must contain at least one product. Please add products to the order."
            );
        }

        var productsIds = body.Details.Select(x => x.ProductId).Distinct().ToList();

        if (body.Details.Any(x => x.Quantity <= 0))
        {
            return Result<OrderDataTransferObject>.FailureResponse(
                "All product quantities must be greater than 0."
            );
        }

        var products = (
            await unitOfWork.ProductRepository.GetAllAsync(
                x => productsIds.Contains(x.Id),
                cancellationToken: cancellationToken
            )
        ).ToList();

        if (products.Count != productsIds.Count)
        {
            var foundProductIds = products.Select(x => x.Id).ToList();
            var missingProductIds = productsIds.Except(foundProductIds).ToList();

            return Result<OrderDataTransferObject>.FailureResponse(
                $"Some products were not found: {string.Join(", ", missingProductIds)}. "
                    + "Please verify all product IDs are correct."
            );
        }

        var productsWithInsufficientStock = new List<string>();
        foreach (var detail in body.Details)
        {
            var product = products.First(x => x.Id == detail.ProductId);
            if (product.Stock < detail.Quantity)
            {
                productsWithInsufficientStock.Add(
                    $"Product '{product.Name}' (ID: {product.Id}) - "
                        + $"Requested: {detail.Quantity}, Available: {product.Stock}"
                );
            }
        }

        if (productsWithInsufficientStock.Any())
        {
            return Result<OrderDataTransferObject>.FailureResponse(
                "Insufficient stock for the following products: "
                    + string.Join("; ", productsWithInsufficientStock)
            );
        }

        var details = new List<OrderDetail>();

        foreach (var product in products)
        {
            var bodyDetail = body.Details.First(x => x.ProductId == product.Id);
            var detailSubtotal = bodyDetail.Quantity * product.Price;
            var detailTax = detailSubtotal * 0.15m;

            var detail = new OrderDetail()
            {
                Quantity = bodyDetail.Quantity,
                ProductId = product.Id,
                Subtotal = detailSubtotal,
                Tax = detailTax,
                Total = detailSubtotal + detailTax,
            };

            details.Add(detail);
        }

        var subtotal = details.Sum(x => x.Subtotal);
        var tax = details.Sum(x => x.Tax);
        var total = details.Sum(x => x.Total);

        var newOrder = new Order()
        {
            ClientId = client.Id,
            Tax = tax,
            Subtotal = subtotal,
            Total = total,
            OrderDetails = details,
        };

        var createdOrder = await unitOfWork.OrderRepository.CreateAsync(
            newOrder,
            cancellationToken
        );

        var createdOrderDto = mapper.Map<OrderDataTransferObject>(createdOrder);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<OrderDataTransferObject>.SuccessResponse(createdOrderDto);
    }
}
