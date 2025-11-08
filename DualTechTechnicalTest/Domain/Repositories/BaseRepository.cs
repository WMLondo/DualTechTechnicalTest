using System.Linq.Expressions;
using DualTechTechnicalTest.Domain.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DualTechTechnicalTest.Domain.Repositories;

public class BaseRepository<TEntity>(AppDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    public async Task<TEntity> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        var existingEntity = await context
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Equals(entity), cancellationToken);

        if (existingEntity is not null)
        {
            throw new InvalidOperationException("The entity already exist in the database.");
        }

        return (await context.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
    }

    public async Task<TEntity> DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        var existingEntity = await context
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Equals(entity), cancellationToken);

        if (existingEntity is null)
        {
            throw new InvalidOperationException("The entity don't exist in the database.");
        }

        return context.Set<TEntity>().Remove(entity).Entity;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        var existingEntity = await context
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Equals(entity), cancellationToken);

        if (existingEntity is null)
        {
            throw new InvalidOperationException("The entity don't exist in the database.");
        }

        return context.Set<TEntity>().Update(entity).Entity;
    }

    public IQueryable<TEntity> All()
    {
        return context.Set<TEntity>();
    }
}
