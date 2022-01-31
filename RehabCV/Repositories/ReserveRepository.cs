using RehabCV.Database;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace RehabCV.Repositories
{
    public class ReserveRepository : IReserve<Reserve>
    {
        public readonly RehabCVContext _context;

        public ReserveRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<Reserve> GetReserve()
        {
            return await _context.Reserves
                                 .AsNoTracking()
                                 .AsQueryable()
                                 .Include(c => c.Children)
                                 .FirstOrDefaultAsync();
        }

        public async Task<string> CreateAsync(Reserve reserve)
        {
            await _context.Reserves.AddAsync(reserve);

            await _context.SaveChangesAsync();

            return reserve.Id;
        }

        public async Task UpdateAsync(string id, Reserve reserve)
        {
            reserve.Id = id;

            _context.Reserves.Update(reserve);

            await _context.SaveChangesAsync();
        }
    }
}
