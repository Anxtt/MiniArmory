using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Web.Models;

using static MiniArmory.Core.Constants.Web;

namespace MiniArmory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICharacterService charService;
        private readonly IMemoryCache memoryCache;

        public HomeController(ICharacterService charService, 
            IMemoryCache memoryCache)
        {
            this.charService = charService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = Cache.HOME_KEY;

            try
            {
                models = this.memoryCache.Get<IEnumerable<CharacterViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.charService.LeaderboardStats();

                    models = models
                        .OrderByDescending(x => x.Rating)
                        .ThenBy(x => x.Name)
                        .Take(3);

                    var options = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        Priority = CacheItemPriority.High
                    };

                    this.memoryCache.Set(cacheKey, models, options);
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