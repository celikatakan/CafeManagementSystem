using System;
using CafeManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CafeManagementSystem.Data.Context
{
	public class CafeAppDbContext : DbContext
	{

        public CafeAppDbContext(DbContextOptions<CafeAppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FeatureConfiguration());
            modelBuilder.ApplyConfiguration(new CafeConfiguration());
            modelBuilder.ApplyConfiguration(new CafeFeatureConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity
                {
                    Id = 1,
                    MaintenanceMode = false,
                });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<FeatureEntity> Features => Set<FeatureEntity>();
        public DbSet<CafeEntity> Cafes => Set<CafeEntity>();
        public DbSet<CafeFeatureEntity> CafeFeatures => Set<CafeFeatureEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();
    }
    public class CafeAppDbContextFactory : IDesignTimeDbContextFactory<CafeAppDbContext>
    {
        public CafeAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CafeAppDbContext>();

            // Buraya kendi bağlantı stringini yaz
            optionsBuilder.UseSqlServer("Server=localhost;Database=CafeManagerDb;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=True;");

            return new CafeAppDbContext(optionsBuilder.Options);
        }
    }
}

