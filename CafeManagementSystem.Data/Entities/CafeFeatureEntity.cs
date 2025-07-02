using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{

        public class CafeFeatureEntity : BaseEntity
        {
            public int CafeId { get; set; }
            public int FeatureId { get; set; }

            public CafeEntity Cafe { get; set; }
            public FeatureEntity Feature { get; set; }
        }

        public class CafeFeatureConfiguration : BaseConfiguration<CafeFeatureEntity>
        {
            public override void Configure(EntityTypeBuilder<CafeFeatureEntity> builder)
            {
                builder.Ignore(x => x.Id);
                builder.HasKey("CafeId", "FeatureId");

                base.Configure(builder);
            }
        }
    
}

