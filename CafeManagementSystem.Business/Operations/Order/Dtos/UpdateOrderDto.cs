using System;
namespace CafeManagementSystem.Business.Operations.Order.Dtos
{
    public record UpdateOrderDto
    {
        public int Id { get; init; }
        public int ProductId { get; init; }
        public int GuestCount { get; init; }
        public bool IsConfirmed { get; init; }
        public DateTime? EndDate { get; init; }
        public string? SpecialRequest { get; init; }
    }

}

