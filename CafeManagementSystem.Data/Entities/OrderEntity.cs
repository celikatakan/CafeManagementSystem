using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManagementSystem.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int GuestCount { get; set; } = 1;
        public bool IsConfirmed { get; set; } = false;
        public string? SpecialRequest { get; set; }


        public UserEntity User { get; set; }
        public ProductEntity Product { get; set; }

    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            base.Configure(builder);
        }
    }
}

