using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Web.Models;

namespace MiniArmory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICharacterService charService;
        private readonly IMemoryCache memoryCache;

        public HomeController(ILogger<HomeController> logger,
            ICharacterService charService, 
            IMemoryCache memoryCache)
        {
            _logger = logger;
            this.charService = charService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = "models";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out models))
                {
                    models = await this.charService.LeaderboardStats();

                    models = models
                        .OrderByDescending(x => x.Rating)
                        .ThenBy(x => x.Name)
                        .Take(3);

                    var cacheExpiryOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(10)
                    };

                    memoryCache.Set(cacheKey, models, cacheExpiryOptions);
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error));
            }

            return this.View(models);
        }

        public IActionResult Privacy() 
            => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}