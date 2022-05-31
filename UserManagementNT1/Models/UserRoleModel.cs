using UserManagementNT1.Areas.Identity.Data;

namespace UserManagementNT1.Models
{
    public class UserRoleModel : AccountUser
    {
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
