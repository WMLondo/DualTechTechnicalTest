using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Repositories.Contracts;

namespace DualTechTechnicalTest.Domain;

public class UnitOfWork(
    AppDbContext context,
    IRepository<Client> clientRepository,
    IRepository<Order> orderRepository,
    IRepository<Product> productRepository,
    IRepository<OrderDetail> orderDetailRepository
) : IUnitOfWork, IDisposable
{
    public IRepository<Client> ClientRepository { get; } = clientRepository;

    public IRepository<Order> OrderRepository { get; } = orderRepository;

    public IRepository<Product> ProductRepository { get; } = productRepository;

    public IRepository<OrderDetail> OrderDetailRepository { get; } = orderDetailRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
