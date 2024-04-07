using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public class BatchJobStatus
    {
        public int Total { get; set; }
        public int TotalPending { get; set; }
        public int TotalCompleted { get; set; }
        public  double? TimeElapsed { get; set; }
        public double? TimeRemaining {  get; set; }
    }
}
