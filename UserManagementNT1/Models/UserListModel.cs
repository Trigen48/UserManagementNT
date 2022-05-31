using Microsoft.AspNetCore.Identity;

namespace UserManagementNT1.Models
{
    public class UserListModel
    {


        public string Search { get; set; }
        public string Role { get; set; }
        public bool ShowDeleted { get; set; }
        public IEnumerable<UserManagementNT1.Areas.Identity.Data.AccountUser> Users { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }
       // public IEm
    }
}
