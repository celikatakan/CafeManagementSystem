using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{
	public class CafeLikeEntity : BaseEntity 
	{
        public int Id { get; set; }
        public int CafeId { get; set; }
        public CafeEntity Cafe { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
     
    }
    public class CafeLikeConfiguration : BaseConfiguration<CafeLikeEntity>
    {
        public override void Configure(EntityTypeBuilder<CafeLikeEntity> builder)
        {
            builder.HasIndex(x => new { x.CafeId, x.UserId }).IsUnique();

            builder.HasOne(x => x.Cafe)
                .WithMany(c => c.Likes)
                .HasForeignKey(x => x.CafeId);

            builder.HasOne(x => x.User)
                .WithMany(u => u.LikedCafes)
                .HasForeignKey(x => x.UserId);

            base.Configure(builder);
        }
    }

}

