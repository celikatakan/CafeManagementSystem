using System;

namespace CafeManagementSystem.Business.Operations.Review.Dtos
{
    public record ReviewDto(
        int Id,
        int CafeId,
        int UserId,
        string UserFullName,
        int Rating,
        string Comment,
        DateTime CreatedDate
    );
}

