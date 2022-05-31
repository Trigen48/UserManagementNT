using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserManagementNT1.Areas.Identity.Data;
using UserManagementNT1.Models;

namespace UserManagementNT1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AccountUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<AccountUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string id, [Bind("FirstName,Surname,Hobbies")] AccountUser userModel)
        {

            if (id == null || _userManager == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {

                user.FirstName = userModel.FirstName;
                user.Surname = userModel.Surname;
                user.Hobbies = userModel.Hobbies;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(user);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}