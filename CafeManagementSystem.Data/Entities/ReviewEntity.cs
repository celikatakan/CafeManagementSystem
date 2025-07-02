using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{
	public class ReviewEntity : BaseEntity
	{
        public int CafeId { get; set; }
        public int UserId { get; set; }
        [Range(1, 5)] public int Rating { get; set; }
        [MaxLength(500)] public string? Comment { get; set; }

        public CafeEntity Cafe { get; set; }
        public UserEntity User { get; set; }
    }
    public class ReviewConfiguration : BaseConfiguration<ReviewEntity>
    {
        public override void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            base.Configure(builder);
        }
    }
}

