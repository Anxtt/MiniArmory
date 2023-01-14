using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data.Models;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Owner, Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<User> userManager;

        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<User> userManager,
            IUserService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task<IActionResult> AllUsers()
        {
            IEnumerable<UserViewModel> models = default;

            try
            {
                models = await userService.AllUsers();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Web.Controllers.HomeController.Error), HOME);
            }

            return View(models);
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> AddRolesToUser(Guid id)
        {
            var user = await userService.GetUserById(id);
            UserRolesViewModel model = default;

            ViewBag.Roles = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList();

            try
            {
                model = await userService.FindUserById(id);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Web.Controllers.HomeController.Error), HOME);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> AddRolesToUser(UserRolesViewModel model)
        {
            var user = await userService.GetUserById(model.Id);
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.Roles?.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.Roles);
                TempData[Temp.MESSAGE] = string.Format(Temp.ADDED_ROLE, user.UserName);
            }
            else
            {
                TempData[Temp.MESSAGE] = string.Format(Temp.REMOVE_ROLE, user.UserName);
            }

            return RedirectToAction(nameof(HomeController.Index), HOME);
        }

        [Authorize(Roles = "Owner")]
        public IActionResult DeleteRoles()
            => View();

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteRoles(RoleFormModel model)
        {
            if (model.Name == null ||
                model.Name == default(Guid).ToString() ||
                model.Name == "-1")
            {
                return this.View(model);
            }

            var id = await roleManager.FindByIdAsync(model.Name);

            try
            {
                await roleManager.DeleteAsync(id);
                TempData[Temp.MESSAGE] = Validation.DELETE_ROLE;
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Web.Controllers.HomeController.Error), HOME);
            }

            return RedirectToAction(nameof(HomeController.Index), HOME);
        }

        public IActionResult CreateRole()
            => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleFormModel model)
        {
            if (await roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), Validation.INVALID_NAME);
                return View(model);
            }

            try
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>()
                {
                    Name = model.Name
                });
                TempData[Temp.MESSAGE] = Validation.CREATE_ROLE;
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Web.Controllers.HomeController.Error), HOME);
            }

            return RedirectToAction(nameof(HomeController.Index), HOME);
        }

        public async Task<IActionResult> GetRoles()
        {
            IEnumerable<JsonFormModel> models = default;

            try
            {
                models = await userService.GetRoles();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Web.Controllers.HomeController.Error), HOME);
            }

            return Json(models);
        }

        public IActionResult WelcomeUser()
            => View();

        [HttpPost]
        public IActionResult WelcomeUser(UserViewModel model)
        {
            return this.RedirectToAction(nameof(GreetUser), new { name = model.Name });
        }

        public IActionResult GreetUser(string name)
        {
            UserViewModel model = new UserViewModel()
            {
                Name = name
            };

            return this.View(model);
        }
    }
}
