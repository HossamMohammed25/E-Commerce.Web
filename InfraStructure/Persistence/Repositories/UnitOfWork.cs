using Domain.Contracts;
using Domain.Models;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _Repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
         var typeName = typeof(TEntity).Name;
            //if(_Repositories.ContainsKey(typeName))
            //    return (GenericRepository<TEntity,TKey>) _Repositories[typeName];
            if (_Repositories.TryGetValue(typeName,out object? Value))
                return (GenericRepository<TEntity,TKey>)Value;

            else
            {
                var Repo = new GenericRepository<TEntity,TKey>(_dbContext);
                _Repositories["typeName"] = Repo ;
                return Repo;
            }

        }

        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
