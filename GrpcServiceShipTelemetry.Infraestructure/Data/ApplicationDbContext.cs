using GrpcServiceShipTelemetry.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceShipTelemetry.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Telemetry> Telemetries { get; set; }

        public string GetConnectionString()
        {
            return this.Database.GetDbConnection().ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Telemetry>(entity =>
            {
                entity.HasKey(e => new { e.ShipId, e.Timestamp });
                entity.Property(e => e.ShipId).HasMaxLength(50);
                entity.Property(e => e.Timestamp).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
