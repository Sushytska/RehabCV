using Microsoft.EntityFrameworkCore;
using RehabCV.Database;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class NumberOfChRepository : INumberOfCh<NumberOfChildren>
    {
        private readonly RehabCVContext _context;

        public NumberOfChRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<NumberOfChildren> GetNumber()
        {
            return await _context.NumberOfChildren.FirstOrDefaultAsync();
        }

        public async Task<string> CreateAsync(NumberOfChildren numberOfChildren)
        {
            await _context.NumberOfChildren.AddAsync(numberOfChildren);

            await _context.SaveChangesAsync();

            return numberOfChildren.Id;
        }

        public async Task UpdateAsync(NumberOfChildren numberOfChildren)
        {
            _context.NumberOfChildren.Update(numberOfChildren);

            await _context.SaveChangesAsync();
        }
    }
}
