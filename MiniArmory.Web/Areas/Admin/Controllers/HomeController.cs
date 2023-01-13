using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Owner, Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IUserService statisticsService;
        private readonly IMemoryCache memoryCache;

        public HomeController(IUserService statisticsService, IMemoryCache memoryCache)
        {
            this.statisticsService = statisticsService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            StatisticsViewModel model = default;
            string cacheKey = Cache.STATISTICS_KEY;

            model = this.memoryCache.Get<StatisticsViewModel>(cacheKey);

            if (model == null)
            {
                model = new StatisticsViewModel();

                model.AchievementsCount = await this.statisticsService.AchievementsCount();
                model.HighestRating = await this.statisticsService.HighestRating();
                model.UsersCount = await this.statisticsService.UsersCount();
                model.MostPopulatedServer = await this.statisticsService.MostPopulatedServer();
                model.MostPlayedClass = await this.statisticsService.MostPlayedClass();
                model.MostPlayedFaction = await this.statisticsService.MostPlayedFaction();
                model.MostPlayedRace = await this.statisticsService.MostPlayedRace();
                model.ArenasCount = await this.statisticsService.ArenasCount();

                var options = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30)
                };

                this.memoryCache.Set(cacheKey, model, options);
            }

            return this.View(model);
        }
    }
}
