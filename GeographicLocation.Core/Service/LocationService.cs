using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILogger<LocationService> _logger;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheService _cacheService;

        public LocationService(ILogger<LocationService> logger, ILocationRepository locationRepository, IMapper mapper, IMemoryCacheService cacheService)
        {
            _logger = logger;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        #region BatchJob
        
        public async Task<BatchJob?> CreateBatchJobAsync(List<IPRequest> IPs)
        {
            try
            {
                var batchJob = new BatchJob { Id = Guid.NewGuid(), TotalIpAddress = IPs.Count, CreatedAt = DateTime.UtcNow };
                await _locationRepository.CreateBatchJobAsync(batchJob);

                foreach (var item in IPs)
                {
                    await _locationRepository.CreateIPAddressAsync(new IPAddress { BatchJobId = batchJob.Id, IP = item.IP, CreatedAt = DateTime.Now });
                }

                await _locationRepository.SaveChangesAsync();

                return batchJob;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<BatchJob?> GetBatchJobByIdAsync(Guid batchJobId)
        {
            return await _locationRepository.GetBatchJobByIdAsync(batchJobId);
        }

        public async Task<BatchJobStatus?> GetBatchJobStatusAsync(Guid batchJobId)
        {
            var batchJob = await _locationRepository.GetBatchJobByIdAsync(batchJobId);
            var IPs = await _locationRepository.GetIPsForBatchJobAsync(batchJobId);

            if (batchJob == null || IPs == null)
            {
                return null;
            }

            var batchStatus = new BatchJobStatus
            {
                Total = batchJob != null ? batchJob.TotalIpAddress : 0,
                TotalPending = IPs.Where(i => i.Processed == false).Count(),
                TotalCompleted = IPs.Where(i => i.Processed == true).Count(),
                TimeElapsed = IPs.Where(i => i.Processed == true).Sum(i => i.TimeElapsed)
            };

            batchStatus.TimeRemaining = (batchStatus.Total - batchStatus.TotalCompleted) / batchStatus.TimeElapsed;
            
            return batchStatus;
        }

        #endregion

        #region IPAddress

        public async Task CreateIPAddressAsync(IPAddress IP)
        {
            await _locationRepository.CreateIPAddressAsync(IP);
        }

        public async Task UpdateIPAddressAsync(IPAddress IP)
        {
            await _locationRepository.UpdateIPAddressAsync(IP);
        }

        #endregion

        public async Task ProcessJob(BatchJob batchJob)
        {
            await Task.Run(async () =>
            {
                var IPs = await _locationRepository.GetIPsForBatchJobAsync(batchJob.Id);

                for (var i = 0; i < IPs.Count(); i++)    
                {
                    var item = IPs.ElementAt(i);

                    if (item.Processed) { continue; }

                    try
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                       
                        var ip = await CreateIPRequest(item.IP);
                        
                        watch.Stop();

                        if (ip != null)
                        {
                            item = _mapper.Map(ip, item);
                               
                            item.UpdatedAt = DateTime.Now;
                            item.Processed = true;
                            item.TimeElapsed = watch.ElapsedMilliseconds;

                            await _locationRepository.UpdateIPAddressAsync(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }

                   // Thread.Sleep(10000);
                }
            });
        }

        public bool IsValidIpAddress(string IP)
        {
            if (System.Net.IPAddress.TryParse(IP, out _))
            {
                return true;
            }
            return false;
        }

        public List<string> GetValidIpAddresses(List<IPRequest> IPs)
        {
            var validList = new List<string>();

            foreach (var item in IPs) 
            {
                if (IsValidIpAddress(item.IP)) 
                {
                    validList.Add(item.IP);
                }
            }

            return validList;
        }

        public async Task<IPAddressDTO?> CreateIPRequest(string IP)
        {
            try
            {
                var cacheKey = $"IP:_{IP}";
                var cachedIP = _cacheService.Get<IPAddressDTO>(cacheKey);

                if (cachedIP != null)
                {
                    return cachedIP;
                }

                IPAddressDTO? result = null;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{HttpClientConstants.GeoLocationApiUrl}?apikey={HttpClientConstants.ApiKey}&ip={IP}");
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        result = DeserializeJson(IP, responseString);
                    }

                    _cacheService.Set(cacheKey, result, TimeSpan.FromHours(1));

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _locationRepository.SaveChangesAsync();
        }

        public IPAddressDTO DeserializeJson (string IP, string responseString)
        {
            var response = JsonConvert.DeserializeObject<IPServiceResponse.Root>(responseString);
            var result = new IPAddressDTO
            {
                IP = IP,
                Latitude = response?.data?.location?.latitude,
                Longitude = response?.data?.location?.longitude,
                CountryName = response?.data?.location?.country?.name,
                CountryCode = response?.data?.location?.country?.alpha2,
                TimeZone = response?.data?.timezone?.id
            };

            return result;
        }
    }
}
