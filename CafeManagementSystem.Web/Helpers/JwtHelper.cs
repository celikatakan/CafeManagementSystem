using System.IdentityModel.Tokens.Jwt;

namespace CafeManagementSystem.Web.Helpers
{
    public static class JwtHelper
    {
        public static (string FirstName, string LastName, string UserType) GetUserInfoFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return (null, null, null);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var firstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            var lastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            var userType = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserType")?.Value
                        ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            return (firstName, lastName, userType);
        }
        public static int GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id");
            return idClaim != null ? int.Parse(idClaim.Value) : 0;
        }
    }
} 