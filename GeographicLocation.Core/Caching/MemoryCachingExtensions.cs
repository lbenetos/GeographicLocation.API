
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public static class MemoryCachingExtensions
    {
        public static IServiceCollection AddMemoryCachingServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var memoryCacheOptions = new MemoryCacheOptions();
            configuration.Bind("MemoryCacheOptions", memoryCacheOptions);

            // Configure the memory cache with options from configuration
            services.AddMemoryCache(options =>
            {
                // Set the size limit of the memory cache
                options.SizeLimit = memoryCacheOptions.SizeLimit;

                // Set the frequency to scan for expired items
                options.ExpirationScanFrequency = memoryCacheOptions.ExpirationScanFrequency;

                // Set the percentage of cache to compact when the size limit is exceeded
                options.CompactionPercentage = memoryCacheOptions.CompactionPercentage;

                // Determines whether to track linked cache entries (used in scenarios like child actions in MVC)
                options.TrackLinkedCacheEntries = memoryCacheOptions.TrackLinkedCacheEntries;

                // Enables tracking of detailed memory cache statistics
                options.TrackStatistics = memoryCacheOptions.TrackStatistics;
            });

            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            return services;
        }
    }
}
