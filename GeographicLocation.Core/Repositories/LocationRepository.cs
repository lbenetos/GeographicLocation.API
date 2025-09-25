using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    /// <summary>
    /// Location Repository
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationContext _context;

        public LocationRepository(LocationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateBatchJobAsync(BatchJob batchJob)
        {
            await _context.BatchJob.AddAsync(batchJob);
        }

        public async Task CreateIPAddressAsync(IPAddress IP)
        {
            await _context.IPAddresses.AddAsync(IP);
        }

        public async Task UpdateIPAddressAsync(IPAddress IP)
        {
            _context.Entry(IP).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task<BatchJob?> GetBatchJobByIdAsync(Guid batchJobId)
        {
            return await _context.BatchJob.Where(i => i.Id == batchJobId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IPAddress>> GetIPsForBatchJobAsync(Guid batchJobId)
        {
            return await _context.IPAddresses.Where(i => i.BatchJobId == batchJobId).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
