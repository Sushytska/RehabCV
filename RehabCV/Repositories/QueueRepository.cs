using RehabCV.Database;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class QueueRepository : IQueue<Queue>
    {
        private readonly RehabCVContext _context;

        public QueueRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<string> AddToQueue(Queue queue)
        {
            await _context.Queues.AddAsync(queue);

            await _context.SaveChangesAsync();

            return queue.Id;
        }
    }
}
