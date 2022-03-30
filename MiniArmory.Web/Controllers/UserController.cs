using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<User> userManager;

        public UserController(RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Roles()
        {
            var user = await userManager.FindByNameAsync(this.User.Identity.Name);

            await userManager.AddToRoleAsync(user, "Owner");

            return RedirectToPage("Home/Index");
        }

        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>()
            {
                Name = "Member"
            });

            return Ok();
        }
    }
}
