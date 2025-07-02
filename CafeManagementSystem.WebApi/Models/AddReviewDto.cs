using System;
using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.WebApi.Models
{
    public class AddReviewDto
    {
        public int CafeId { get; set; }
        public int UserId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [MaxLength(500)]
        public string Comment { get; set; }
    }
}

