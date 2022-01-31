using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IRepository<TEntity>
    {
        public Task<string> CreateAsync(TEntity entity);
        public Task UpdateAsync(string id, TEntity entity);
    }
}
