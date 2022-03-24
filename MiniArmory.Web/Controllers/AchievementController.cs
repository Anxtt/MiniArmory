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
        {
            return View();
        }
        [HttpPost]

        public IActionResult AddAchievement(AchievementFormModel model)
        {
            this.achievementService.Add(model);

            return View();
        }
    }
}
