using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IEvent<TEntity> : IRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindAll();
        public Task<TEntity> FindById(string id);
        public Task<int> DeleteAsync(string id);
    }
}
