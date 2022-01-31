using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IGroup<TEntity> : IEvent<TEntity>
    {
        public Task<TEntity> FindByName(string name);
    }
}
