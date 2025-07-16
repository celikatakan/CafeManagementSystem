using System;
namespace CafeManagementSystem.WebApi.Models
{
	public class LoginResponse
	{
       public string Message { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }

    }
}

