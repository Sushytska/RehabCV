using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface INumberOfCh<TEntity>
    {
        public Task<TEntity> GetNumber();
        public Task<string> CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
    }
}
