using RehabCV.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;
using RehabCV.Interfaces;

namespace RehabCV.Repositories
{
    public class RehabRepository : IRehabilitation<Rehabilitation>
    {
        private readonly RehabCVContext _context;

        public RehabRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<Rehabilitation> FindByChildId(string id)
        {
            return await _context.Rehabilitations
                                            .FirstOrDefaultAsync(x => x.ChildId == id);
        }

        public async Task<string> CreateAsync(Rehabilitation rehab)
        {
            await _context.Rehabilitations.AddAsync(rehab);

            await _context.SaveChangesAsync();

            return rehab.ChildId;
        }

        public async Task UpdateAsync(string id, Rehabilitation rehabilitation)
        {
            rehabilitation.Id = id;

            _context.Rehabilitations.Update(rehabilitation);

            await _context.SaveChangesAsync();
        }
    }
}
