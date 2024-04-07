using System;
using System.Collections.Generic;

namespace GeographicLocation.Core;

public partial class IPAddress
{
    public int Id { get; set; }

    public string IP { get; set; } = null!;

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }

    public string? TimeZone { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public Guid BatchJobId { get; set; }

    public bool Processed { get; set; }

    public double? TimeElapsed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BatchJob BatchJob { get; set; } = null!;
}
