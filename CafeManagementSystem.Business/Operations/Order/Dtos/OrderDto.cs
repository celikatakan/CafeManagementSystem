using System;
namespace CafeManagementSystem.Business.Operations.Order.Dtos
{
    public record OrderDto(
    int Id,
    int ProductId,
    string ProductName,
    int UserId,
    string UserFullName,
    DateTime StartDate,
    DateTime? EndDate,
    int GuestCount,
    bool IsConfirmed,
    string? SpecialRequest
);
}

