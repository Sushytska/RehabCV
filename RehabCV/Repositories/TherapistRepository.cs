using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using RehabCV.Interfaces;

namespace RehabCV.Repositories
{
    public class TherapistRepository:ITherapist<Therapist>
    {
        private readonly RehabCVContext _context;

        public TherapistRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public Task<string> CreateAsync(Therapist entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Therapist>> FindAll()
        {
            return await _context.Therapists.ToListAsync();
        }

        public Task<Therapist> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, Therapist entity)
        {
            throw new NotImplementedException();
        }
    }
}
