using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core.Service
{
    public interface ILocationService
    {
        #region Batch Job

        Task<BatchJob?> CreateBatchJobAsync(List<IPRequest> batchJob);

        Task<BatchJob?> GetBatchJobByIdAsync(Guid batchJobId);

        Task<BatchJobStatus?> GetBatchJobStatusAsync(Guid batchJobId);

        #endregion

        #region IPAddress
        Task<IPAddressDTO?> CreateIPRequest(string IP);

        Task CreateIPAddressAsync(IPAddress IP);

        Task UpdateIPAddressAsync(IPAddress IP);

        #endregion

        Task ProcessJob(BatchJob batchJob);

        bool IsValidIpAddress(string IP);

        List<string> GetValidIpAddresses(List<IPRequest> IPs);

        IPAddressDTO DeserializeJson(string IP, string responseString);

        Task<bool> SaveChangesAsync();
    }
}
