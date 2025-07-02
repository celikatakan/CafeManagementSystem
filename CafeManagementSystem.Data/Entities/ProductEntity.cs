using System;
using CafeManagementSystem.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public int CafeId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int? StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<OrderEntity> Orders { get; set; }
        public CafeEntity Cafe { get; set; }
    }

    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.ProductName)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(x => x.Price)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Category)
              .HasMaxLength(50);

            base.Configure(builder);
        }
    }
}

