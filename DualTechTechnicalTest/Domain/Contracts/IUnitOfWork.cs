using DualTechTechnicalTest.Domain.Entities;
using DualTechTechnicalTest.Domain.Repositories.Contracts;

namespace DualTechTechnicalTest.Domain.Contracts;

public interface IUnitOfWork
{
    public IRepository<Client> ClientRepository { get; }

    public IRepository<Order> OrderRepository { get; }

    public IRepository<Product> ProductRepository { get; }

    public IRepository<OrderDetail> OrderDetailRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}