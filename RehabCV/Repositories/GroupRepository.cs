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
    public class GroupRepository : IGroup<Group>
    {
        private readonly RehabCVContext _context;

        public GroupRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<IEnumerable<Group>> FindAll()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> FindById(string id)
        {
            return await _context.Groups
                                 .AsNoTracking()
                                 .AsQueryable()
                                 .Include(c => c.Children)
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> FindByName(string name)
        {
            return await _context.Groups
                                 .AsNoTracking()
                                 .AsQueryable()
                                 .Include(c => c.Children)
                                 .FirstOrDefaultAsync(x => x.NameOfDisease == name);
        }

        public async Task<string> CreateAsync(Group group)
        {
            await _context.Groups.AddAsync(group);

            await _context.SaveChangesAsync();

            return group.Id;
        }

        public async Task UpdateAsync(string id, Group group)
        {
            group.Id = id;

            _context.Groups.Update(group);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;
            var group = _context.Groups.FirstOrDefault(x => x.Id == id);

            if (group != null)
            {
                _context.Groups.Remove(group);

                 result = await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
