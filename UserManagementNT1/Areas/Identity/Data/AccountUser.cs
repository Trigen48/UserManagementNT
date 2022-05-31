using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserManagementNT1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AccountUser class
public class AccountUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Surname { get; set; }

    [PersonalData]
    [Column(TypeName = "int")]
    public int Age { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(500)")]
    public string Hobbies { get; set; }

    [Column(TypeName = "bit")]
    public bool Deleted { get; set; }

    public string DeletedString
    {
        get
        {
            return this.Deleted ? "Yes" : "No";
        }
    }


}

