using System.Linq.Expressions;
using DualTechTechnicalTest.Domain.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DualTechTechnicalTest.Domain.Repository;

public class BaseRepository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : class
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

            var addedEntity = context.Set<TEntity>().Add(entity);

            var result = await context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return addedEntity.Entity;
            }

            throw new ArgumentException("Something went wrong saving the entity in the database.");
        }

        public async Task<bool> CreateRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default
        )
        {
            var existingEntity = await context
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => entities.Any(y => y.Equals(x)),
                    cancellationToken
                );

            if (existingEntity is not null)
            {
                throw new InvalidOperationException(
                    "One or more entities already exist in the database."
                );
            }

            context.Set<TEntity>().AddRange(entities);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        public async Task<TEntity> DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            var existingEntity =
                await context
                    .Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Equals(entity), cancellationToken);

            if (existingEntity is null)
            {
                throw new InvalidOperationException("The entity don't exist in the database.");
            }

            var deletedEntity = context.Set<TEntity>().Remove(entity);

            var result = await context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return deletedEntity.Entity;
            }

            throw new ArgumentException("Something went wrong saving the entity in the database.");
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
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public IQueryable<TEntity> All()
        {
            return context.Set<TEntity>();
        }

        public Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return query.CountAsync(cancellationToken);
        }
}