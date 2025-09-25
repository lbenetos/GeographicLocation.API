using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeographicLocation.Core;

public partial class LocationContext : DbContext
{
    public LocationContext()
    {
    }

    public LocationContext(DbContextOptions<LocationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BatchJob> BatchJob { get; set; }

    public virtual DbSet<IPAddress> IPAddresses { get; set; }

    /*  */
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //    => optionsBuilder.UseSqlServer("Data Source=Lambros-PC-2; Initial Catalog=IPLocation; MultipleActiveResultSets=true; Trusted_Connection=true; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BatchJob>(entity =>
        {
            entity.ToTable("BatchJob");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IPAddress>(entity =>
        {
            entity.ToTable("IPAddress");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IP)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.TimeZone)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.BatchJob).WithMany(p => p.IPAddresses)
                .HasForeignKey(d => d.BatchJobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IPAddress_BatchJob");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
