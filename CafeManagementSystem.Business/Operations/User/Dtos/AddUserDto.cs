using System;
namespace CafeManagementSystem.Business.Operations.User.Dtos
{
	public class AddUserDto
	{
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

