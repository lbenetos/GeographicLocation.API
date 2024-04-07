using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public interface IMemoryCacheService
    {
        T Get<T>(string key);

        void Set<T>(string key, T value, TimeSpan cacheDuration);

        bool Remove(string key);
    }
}
