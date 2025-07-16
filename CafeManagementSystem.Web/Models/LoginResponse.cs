namespace CafeManagementSystem.Web.Models
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
} 