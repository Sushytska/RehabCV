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
    public class ChildRepository : IChild<Child>
    {
        private readonly RehabCVContext _context;

        public ChildRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<IEnumerable<Child>> FindAll()
        {
            return await _context.Children.ToListAsync();
        }

        public async Task<Child> FindById(string id)
        {
            return await _context.Children.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Child>> FindByParentId(string parentId)
        {
            return await _context.Children
                                    .AsNoTracking()
                                    .AsQueryable()
                                    .Where(x => x.UserId == parentId).ToListAsync();
            
        }

        public async Task<string> CreateAsync(Child child)
        {
            await _context.Children.AddAsync(child);

            await _context.SaveChangesAsync();

            return child.Id;
        }

        public async Task UpdateAsync(string id, Child child)
        {
            child.Id = id;

            _context.Children.Update(child);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;
            var child = _context.Children.FirstOrDefault(x => x.Id == id);

            if (child != null)
            {
                _context.Children.Remove(child);

                result =  await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
