using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class AchievementController : Controller
    {
        private readonly IAchievementService achievementService;

        public AchievementController(IAchievementService achievementService) 
            => this.achievementService = achievementService;

        public IActionResult AddAchievement() 
            => this.View();

        [HttpPost]
        public async Task<IActionResult> AddAchievement(AchievementFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.achievementService.Add(model);

            return this.View();
        }
    }
}
