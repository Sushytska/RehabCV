using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IReserve<TEntity> : IRepository<TEntity>
    {
        public Task<TEntity> GetReserve();
    }
}
