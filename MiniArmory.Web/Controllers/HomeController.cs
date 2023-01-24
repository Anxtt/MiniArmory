using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Web.Models;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRedisService redis;

        private readonly ICharacterService charService;

        public HomeController(ICharacterService charService,
            IRedisService redis)
        {
            this.charService = charService;
            this.redis = redis;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = Cache.HOME_KEY;

            try
            {
                models = await this.redis.RetrieveCache<IEnumerable<CharacterViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.charService.LeaderboardStats();

                    models = models
                        .OrderByDescending(x => x.Rating)
                        .ThenBy(x => x.Name)
                        .Take(3);

                    await this.redis.SetCache(cacheKey, models);
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error));
            }

            return this.View(models);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}