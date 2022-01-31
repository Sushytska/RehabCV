using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IQueue<TEntity>
    {
        public Task<string> AddToQueue(TEntity entity);
    }
}
