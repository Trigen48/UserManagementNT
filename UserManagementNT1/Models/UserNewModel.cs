using Microsoft.AspNetCore.Identity;

namespace UserManagementNT1.Models
{
    public class UserNewModel
    {
        public UserManagementNT1.Models.UserRoleModel User { get; set; }
        public IEnumerable<IdentityRole> UserRoles { get; set; }
    }
}
