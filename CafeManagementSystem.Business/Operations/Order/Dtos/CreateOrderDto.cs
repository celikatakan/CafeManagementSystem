using System;
using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.Business.Operations.Order.Dtos
{
    public record CreateOrderDto
    {
        [Required] public int ProductId { get; init; }
        [Required] public int UserId { get; init; }
        [Range(1, 10)] public int GuestCount { get; init; } = 1;
        public DateTime? StartDate { get; init; }
        public string? SpecialRequest { get; init; }
    }
}

