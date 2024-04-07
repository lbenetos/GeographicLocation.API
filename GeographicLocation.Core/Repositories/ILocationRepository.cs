using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public interface ILocationRepository
    {
        Task CreateBatchJobAsync(BatchJob batchJob);

        Task<BatchJob?> GetBatchJobByIdAsync(Guid batchJobId);

        Task<IEnumerable<IPAddress>> GetIPsForBatchJobAsync(Guid batchJobId);

        Task CreateIPAddressAsync(IPAddress IP);

        Task UpdateIPAddressAsync(IPAddress IP);

        Task<bool> SaveChangesAsync();
    }
}
