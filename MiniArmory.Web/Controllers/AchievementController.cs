using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class AchievementController : Controller
    {
        private readonly IMemoryCache memoryCache;
        
        private readonly IAchievementService achievementService;

        public AchievementController(IAchievementService achievementService, IMemoryCache memoryCache)
        {
            this.achievementService = achievementService;
            this.memoryCache = memoryCache;
        }

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
                TempData["Message"] = "Created achievement successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AllAchievements));
        }


        public async Task<IActionResult> AllAchievements()
        {
            IEnumerable<AchievementViewModel> models = default;
            string cacheKey = "allAchiesKey";

            try
            {
                models = this.memoryCache.Get<IEnumerable<AchievementViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.achievementService.AllAchievements();

                    this.memoryCache.Set(cacheKey, models, DateTimeOffset.Now.AddDays(1));
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }
    }
}
