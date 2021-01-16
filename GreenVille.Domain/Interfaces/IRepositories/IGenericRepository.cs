using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IRepositories
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<bool> Exists(object key);

        Task<int> AddAsync(TEntity entity);

        Task<TEntity> GetAsync(object key);

        Task<TEntity> GetAsync(params object[] keys);

        IQueryable<TEntity> QueryAll(Func<TEntity, bool> predicate);

        IQueryable<TEntity> QueryAll();

        Task<IList<TEntity>> ListAllAsync();

        IList<TEntity> ListAll(Func<TEntity, bool> predicate);

        void Update(TEntity entity);

        void Update(TEntity entity, string property);

        void Delete(Func<TEntity, bool> predicate);

        long SaveAll();

        Task<long> SaveAllAsync();

    }
}
