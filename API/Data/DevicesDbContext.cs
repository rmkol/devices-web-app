using System.Threading;
using System.Threading.Tasks;
using API.Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Data
{
	public interface IDevicesDbContext
	{
		DbSet<Device> Devices { get; set; }
		DbSet<DeviceDetails> DeviceDetails { get; set; }
		DbSet<DeviceType> DeviceTypes { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

		Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
	}

	public class DevicesDbContext : DbContext, IDevicesDbContext
	{
		public DbSet<Device> Devices { get; set; }
		public DbSet<DeviceDetails> DeviceDetails { get; set; }
		public DbSet<DeviceType> DeviceTypes { get; set; }

		protected DevicesDbContext() { }
		public DevicesDbContext(DbContextOptions options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=");

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DeviceType>(
				builder => { builder.ToTable("device_type", "devices"); });

			modelBuilder.Entity<DeviceDetails>(
				builder =>
				{
					builder.ToTable("device", "devices");
					builder.Property(e => e.Status)
						.HasConversion(new EnumToLowerCaseStringConverter<DeviceStatus>());
				});

			modelBuilder.Entity<Device>(
				builder =>
				{
					builder.ToTable("device", "devices");
					builder.Property(e => e.Status)
						.HasConversion(new EnumToLowerCaseStringConverter<DeviceStatus>());
					builder.HasOne(o => o.DeviceDetails).WithOne()
						.HasForeignKey<DeviceDetails>(o => o.Id);
				});
		}
	}
}