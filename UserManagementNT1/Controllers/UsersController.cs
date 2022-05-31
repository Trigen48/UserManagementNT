using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementNT1.Areas.Identity.Data;
using UserManagementNT1.Data;
using UserManagementNT1.Models;

namespace UserManagementNT1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<AccountUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

       /* private readonly UserManager<AccountUser> _userManager;*/
        private readonly IUserStore<AccountUser> _userStore;
        private readonly IUserEmailStore<AccountUser> _emailStore;
        public UsersController(UserManager<AccountUser> userManager, RoleManager<IdentityRole> roleManager, IUserStore<AccountUser> userStore)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore= GetEmailStore();

        }

        private IUserEmailStore<AccountUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AccountUser>)_userStore;
        }
        // GET: UsersController
        public ActionResult Index(string role = "", string search = "", bool showDeleted = false)
        {
            //return View();

            var userData = new UserListModel() { Search=search, Role=role,ShowDeleted=showDeleted};


            userData.UserRoles = _roleManager.Roles.ToList();
            if (role != null && role.Length > 0)
            {
                var result =   _userManager.GetUsersInRoleAsync(role);
                userData.Users = result.Result;
            }
            else
            {
                userData.Users = _userManager.Users.ToList();
            }

            if (search != null && search.Length > 0)
            {
                userData.Users = userData.Users.Where(x => (x.FirstName.ToLower().Contains(search.ToLower()) || x.Surname.ToLower().Contains(search.ToLower())) && (x.Deleted == showDeleted)).ToList();
            }
            else
            {
                userData.Users = userData.Users.Where(x => (x.Deleted == showDeleted));
            }

            return View(userData);
        }

        // GET: UsersController/Details/5
        public async Task<IActionResult> ViewDetails(string id)
        {
            if (id == null || _userManager == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UsersController/Create
        public ActionResult New()
        {
            return View(new UserNewModel() { User = null, UserRoles = _roleManager.Roles.ToList() });
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind("FirstName,Surname,Age,Email,PhoneNumber,Hobbies,Role,Password")] UserRoleModel user)
        {
            
            if (ModelState.IsValid)
            {
                await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, user.Password);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var roleResult = await _roleManager.FindByNameAsync(user.Role);

                    if (roleResult != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, roleResult.Name);
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


                if (result.Errors.Count()<1)
                return RedirectToAction(nameof(Index));
            }
            return View(new UserNewModel() { User = user, UserRoles = _roleManager.Roles.ToList() });
        }

        // GET: UsersController/Edit/
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _userManager == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,Surname,Age,PhoneNumber,Hobbies")] AccountUser userModel)
        {

            if (id == null || _userManager == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                user.FirstName = userModel.FirstName;
                user.Surname = userModel.Surname;
                user.Age = userModel.Age;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Hobbies = userModel.Hobbies;

               var result= await _userManager.UpdateAsync(user);

                if(result.Succeeded)
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

        // GET: UsersController/Delete/
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _userManager==null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            var lg = await _userManager.GetUserAsync(User);


            if (user == null || user.Deleted || lg.Id == user.Id)
            {
                return NotFound();
            }

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager == null)
            {
                return Problem("UserManager is null.");
            }
            var user = await _userManager.FindByIdAsync(id);

            var lg = await _userManager.GetUserAsync(User);


            if (user == null || user.Deleted || lg.Id == user.Id)
            {
                return NotFound();
            }

            if (user != null)
            {
                user.Deleted = true;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: UsersController/Restore/
        public async Task<IActionResult> Restore(string? id)
        {
            if (id == null || _userManager == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            var lg = await _userManager.GetUserAsync(User);


            if (user == null || !user.Deleted || lg.Id == user.Id)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Restore/
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(string id)
        {
            if (_userManager == null)
            {
                return Problem("UserManager is null.");
            }
            var user = await _userManager.FindByIdAsync(id);

            var lg = await _userManager.GetUserAsync(User);


            if (user == null || !user.Deleted || lg.Id == user.Id)
            {
                return NotFound();
            }

            if (user != null)
            {
                user.Deleted = false;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
