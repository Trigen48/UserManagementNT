using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserManagementNT1.Data;
using UserManagementNT1.Models;

namespace UserManagementNT1.Controllers
{
    [Authorize(Roles ="Admin,Reporter")]
    public class AuditLogsController : Controller
    {
        private readonly UserManagementNT1Context _context;

        public AuditLogsController(UserManagementNT1Context context)
        {
            _context = context;
        }

        // GET: AuditLogs
        public async Task<IActionResult> Index(String Search,DateTime filterDate)
        {
              return _context.AspNetAuditLog != null ? 
                          View(await _context.AspNetAuditLog.ToListAsync()) :
                          Problem("Entity set 'UserManagementNT1Context.AspNetAuditLog'  is null.");
        }


    }
}
