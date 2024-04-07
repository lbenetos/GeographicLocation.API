using System;
using System.Collections.Generic;

namespace GeographicLocation.Core;

public partial class BatchJob
{
    public Guid Id { get; set; }

    public int TotalIpAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<IPAddress> IPAddresses { get; set; } = new List<IPAddress>();
}
