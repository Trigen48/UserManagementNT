using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementNT1.Areas.Identity.Data;
using UserManagementNT1.Models;

namespace UserManagementNT1.Data;

public class UserManagementNT1Context : IdentityDbContext<AccountUser>
{
    public UserManagementNT1Context(DbContextOptions<UserManagementNT1Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<UserManagementNT1.Models.AspNetAuditLog> AspNetAuditLog { get; set; }
}
