using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{
    public class CafeEntity : BaseEntity
    {
        public string Name { get; set; }
        public int? Stars { get; set; }
        public string Location { get; set; }

        public ICollection<CafeFeatureEntity> CafeFeatures { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public ICollection<CafeLikeEntity> Likes { get; set; }
    }
    public class CafeConfiguration : BaseConfiguration<CafeEntity>
    {
        public override void Configure(EntityTypeBuilder<CafeEntity> builder)
        {
            builder.Property(x => x.Stars)
                 .IsRequired(false);
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            base.Configure(builder);
        }
    }
}

