using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementNT1.Models
{
    public class AspNetAuditLog
    {
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(256)")]
        public string Email { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AuditDate { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string UserId { get; set; }
    }
}
