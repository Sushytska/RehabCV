using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IChild<TEntity> : IEvent<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindByParentId(string id);
    }
}
