using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected readonly DataContext _context;

        protected readonly TEntity _entity;


        public GenericRepository(DataContext context) => this._context = context;


        public async Task<bool> Exists(object key)
        {
            var result = await _context.Set<TEntity>().FindAsync(key);
            return (result != null);
        }


        public async Task<TEntity> GetAsync(object key) => await _context.Set<TEntity>().FindAsync(key);

        public async Task<TEntity> GetAsync(params object[] keys) => await _context.Set<TEntity>().FindAsync(keys);

        public IQueryable<TEntity> QueryAll(Func<TEntity, bool> predicate) => _context.Set<TEntity>().Where(predicate).AsQueryable();

        public IQueryable<TEntity> QueryAll() => _context.Set<TEntity>().AsQueryable();

        public async Task<IList<TEntity>> ListAllAsync() => await _context.Set<TEntity>().ToListAsync();

        public IList<TEntity> ListAll(Func<TEntity, bool> predicate) => _context.Set<TEntity>().Where(predicate).ToList();


        public async Task<int> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }


        public void Update(TEntity entity) => _context.Entry(entity).State = EntityState.Modified;

        public void Update(TEntity entity, string property) => _context.Entry(entity).Property(property).IsModified = true;


        public void Delete(Func<TEntity, bool> predicate)
            => _context.Set<TEntity>().Where(predicate).ToList().ForEach(delete => _context.Set<TEntity>().Remove(delete));


        public long SaveAll() => _context.SaveChanges();

        public async Task<long> SaveAllAsync() => await _context.SaveChangesAsync();


        #region [ Dispose ]

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GenericRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
