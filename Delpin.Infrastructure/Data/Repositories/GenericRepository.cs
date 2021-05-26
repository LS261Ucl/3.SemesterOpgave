using Delpin.Application.Interfaces;
using Delpin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Delpin.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DelpinContext _context;

        public GenericRepository(DelpinContext context)
        {
            _context = context;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(criteria);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> criteria = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string[]> UpdateConcurrentlyAsync(T entity, byte[] rowVersion)
        {
            _context.Entry(entity).Property("RowVersion").OriginalValue = rowVersion;

            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return new[] { "" };
            }
            catch (DbUpdateConcurrencyException e)
            {
                return new[]
                {
                    "Den entitet, som du forsøgte at redigere," +
                    " blev ændret af en anden bruger, efter at du fik den oprindelige værdi. " +
                    "Redigeringshandlingen blev annulleret. Hvis du stadig vil redigere denne entitet, " +
                    "så tryk tilbage og find entiteten igen.",
                    e.Message, "409"
                };
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public void DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}