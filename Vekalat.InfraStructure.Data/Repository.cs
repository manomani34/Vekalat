using Vekalat.Application.Common;
using Vekalat.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Vekalat.InfraStructure.Data
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly VekalatDataContext _context;
        private IDbContextTransaction _transaction;
        private readonly DbSet<T> dbSet;

        public Repository(VekalatDataContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        #region Transaction
        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }
        public void Commit()
        {
            _transaction.Commit();
        }
        public void Rollback()
        {
            _transaction.Rollback();
        }
        public void Dispose()
        {
            _transaction.Dispose();
        }
        #endregion


        protected IQueryable<T> GetQueryable(bool tracked, string includes)
        {
            var query = (tracked) ? dbSet.AsQueryable() : dbSet.AsNoTracking();
            if (!string.IsNullOrEmpty(includes))
            {
                var includesList = includes.Split(',');

                foreach (var include in includesList)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
        public async Task<List<T>> GetAll(bool tracked = false, string includes = "")
            => await GetQueryable(tracked, includes).ToListAsync();

        public async Task<T> GetById(int id, bool tracked = false, string includes = "")
            => await GetQueryable(tracked, includes).SingleOrDefaultAsync(c => c.Id == id);

        public async Task<T> InsertNew(T newEntity)
        {
            await dbSet.AddAsync(newEntity);
            return newEntity;
        }

        public async Task Update(T updateEntity)
        {
            await Task.FromResult(dbSet.Update(updateEntity));
        }

        public async Task Delete(int id)
        {
            var items = await dbSet.FindAsync(id);
            dbSet.Remove(items);
        }

        public async Task Delete(T deleteEntity)
        {
            await Task.FromResult(dbSet.Remove(deleteEntity));
        }

        public async Task DeleteRang(List<T> deleteEntities)
        {
            dbSet.RemoveRange(deleteEntities);
            await Task.CompletedTask;
        }

        public async Task SoftDelete(int id)
        {
            var items = await dbSet.FindAsync(id);
            items.IsDeleted = true;
            dbSet.Update(items);
        }
        public async Task SoftDelete(T deleteEntity)
        {
            deleteEntity.IsDeleted = true;
            await Task.FromResult(dbSet.Update(deleteEntity));
        }

        public async Task SoftDeleteRang(List<T> deleteEntities)
        {
            deleteEntities.ForEach(c => c.IsDeleted = true);
            dbSet.UpdateRange(deleteEntities);
            await Task.CompletedTask;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            return await dbSet.AnyAsync(c => c.Id == id);
        }
    }
}
