using System.Threading;
using System.Threading.Tasks;
using API.Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Data
{
	public interface IDevicesDbContext
	{
		DbSet<Role> Roles { get; set; }
		DbSet<User> Users { get; set; }
		DbSet<UserRole> UserRoles { get; set; }
		DbSet<Token> Tokens { get; set; }
		DbSet<Device> Devices { get; set; }
		DbSet<DeviceDetails> DeviceDetails { get; set; }
		DbSet<DeviceType> DeviceTypes { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

		Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
			where TEntity : class;
	}

	public class DevicesDbContext : DbContext, IDevicesDbContext
	{
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<Token> Tokens { get; set; }
		public DbSet<Device> Devices { get; set; }
		public DbSet<DeviceDetails> DeviceDetails { get; set; }
		public DbSet<DeviceType> DeviceTypes { get; set; }

		protected DevicesDbContext() { }
		public DevicesDbContext(DbContextOptions options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password="); // todo: move to config

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Role>(
				builder => { builder.ToTable("role", "devices"); });

			modelBuilder.Entity<User>(
				builder => { builder.ToTable("user", "devices"); });

			modelBuilder.Entity<UserRole>(
				builder =>
				{
					builder.ToTable("user_role", "devices");
					builder.HasKey(ur => new { ur.UserId, ur.RoleId });
				});

			modelBuilder.Entity<Token>(
				builder => { builder.ToTable("token", "devices"); });

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