using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class AchievementController : Controller
    {
        private readonly IAchievementService achievementService;

        public AchievementController(IAchievementService achievementService)
            => this.achievementService = achievementService;

        [Authorize(Roles = "Admin, Owner")]
        public IActionResult AddAchievement()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> AddAchievement(AchievementFormModel model)
        {
            if (!ModelState.IsValid)
            {
                if (await this.achievementService.DoesExist(model.Name))
                {
                    ModelState.AddModelError("Name", "Invalid Name");
                }

                return this.View(model);
            }

            try
            {
                await this.achievementService.Add(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.RedirectToAction(nameof(AllAchievements));
        }


        public async Task<IActionResult> AllAchievements()
        {
            IEnumerable<AchievementViewModel> achies = default;

            try
            {
                achies = await this.achievementService.AllAchievements();
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.View(achies);
        }
    }
}
