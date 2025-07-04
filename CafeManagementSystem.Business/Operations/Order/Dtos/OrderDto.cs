using System;
namespace CafeManagementSystem.Business.Operations.Order.Dtos
{
    public record OrderDto(
    int Id,
    int ProductId,
    string ProductName,
    int UserId,
    string UserFullName,
    DateTime CreatedDate,
    int GuestCount,
    bool IsConfirmed,
    string? SpecialRequest
);
}

