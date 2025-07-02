using System;
using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.Business.Operations.Review.Dtos
{
    public record CreateReviewDto
    {
        public int CafeId { get; init; }
        public int UserId { get; init; }
        public int Rating { get; init; }

      
        public string Comment { get; init; } = string.Empty;

       
     
    }
}

