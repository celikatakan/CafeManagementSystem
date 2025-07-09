using System;
using System.Collections.Generic;
using CafeManagementSystem.Data.Enums;
using CafeManagementSystem.Business.Operations.Review.Dtos;

namespace CafeManagementSystem.Business.Operations.User.Dtos
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType UserType { get; set; }
        public string Username => $"{FirstName} {LastName}";
        public DateTime CreatedAt { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}

