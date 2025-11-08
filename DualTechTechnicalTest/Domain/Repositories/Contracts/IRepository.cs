using System.Linq.Expressions;

namespace DualTechTechnicalTest.Domain.Repositories.Contracts;

public interface IRepository<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false,
        CancellationToken cancellationToken = default
    );

    IQueryable<TEntity> All();

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
