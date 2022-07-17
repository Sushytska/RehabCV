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

        public async Task<string> CreateAsync(Therapist therapist)
        {
            await _context.Therapists.AddAsync(therapist);

            await _context.SaveChangesAsync();

            return therapist.Id;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;
            var therapist = _context.Therapists.FirstOrDefault(x => x.Id == id);

            if (therapist != null)
            {
                _context.Therapists.Remove(therapist);

                result = await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<Therapist>> FindAll()
        {
            return await _context.Therapists.ToListAsync();
        }

        public async Task<Therapist> FindById(string id)
        {
            return await _context.Therapists.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(string id, Therapist therapist)
        {
            therapist.Id = id;

            _context.Therapists.Update(therapist);

            await _context.SaveChangesAsync();
        }
    }
}
