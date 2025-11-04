using Microsoft.EntityFrameworkCore;
using RetailManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Repositories
{
    public class Repository<TEntity>(RetailDbContext dbContext) : IRepository<TEntity> where TEntity : class
    {
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            dbContext.Set<TEntity>().Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Set<TEntity>().ToListAsync<TEntity>(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
           return await dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }

    }


}
