using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Areas.OwnerAdmin.Controllers
{
    [Authorize(Roles = "Owner, Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IUserService statisticsService;

        public HomeController(IUserService statisticsService) 
            => this.statisticsService = statisticsService;

        public async Task<IActionResult> Index()
        {
            StatisticsViewModel model = new StatisticsViewModel();

            model.AchievementsCount = await this.statisticsService.AchievementsCount();
            model.HighestRating = await this.statisticsService.HighestRating();
            model.UsersCount = await this.statisticsService.UsersCount();
            model.MostPopulatedServer = await this.statisticsService.MostPopulatedServer();
            model.MostPlayedClass = await this.statisticsService.MostPlayedClass();
            model.MostPlayedFaction = await this.statisticsService.MostPlayedFaction();
            model.MostPlayedRace = await this.statisticsService.MostPlayedRace();
            model.ArenasCount = await this.statisticsService.ArenasCount();

            return this.View(model);
        }
    }
}
