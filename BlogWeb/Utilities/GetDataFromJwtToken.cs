using DataLayer.Entities.Users;
using System.Data;
using System.Security.Claims;

namespace BlogWeb.Utilities
{
    public static class GetDataFromJwtToken
    {
        public static List<string> GetRole(this ClaimsPrincipal principal)
        {
            List<string> roles = new List<string>();
            foreach (var item in principal.FindAll(ClaimTypes.Role))
            {
                roles.Add(item.Value);
            }
            return roles;
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static string GetUserFullName(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
