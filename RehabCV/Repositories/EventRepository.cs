using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RehabCV.Interfaces;

namespace RehabCV.Repositories
{
    public class EventRepository : IEvent<Event>
    {
        private readonly RehabCVContext _context;

        public EventRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<IEnumerable<Event>> FindAll()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> FindById(string id)
        {
            return await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string> CreateAsync(Event @event)
        {
            await _context.Events.AddAsync(@event);

            await _context.SaveChangesAsync();

            return @event.Id;
        }

        public async Task UpdateAsync(string id, Event e)
        {
            e.Id = id;

            _context.Events.Update(e);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;

            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (@event != null)
            {
                _context.Events.Remove(@event);

                result = await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
