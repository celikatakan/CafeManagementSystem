using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{

   public class FeatureEntity : BaseEntity
        {
            public string Title { get; set; }
            public ICollection<CafeFeatureEntity> CafeFeatures { get; set; }
        }

        public class FeatureConfiguration : BaseConfiguration<FeatureEntity>
        {
            public override void Configure(EntityTypeBuilder<FeatureEntity> builder)
            {
                base.Configure(builder);
            }
        }
}

