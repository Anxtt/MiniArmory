using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Web.Models;

namespace MiniArmory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICharacterService charService;

        public HomeController(ILogger<HomeController> logger, 
            ICharacterService charService)
        {
            _logger = logger;
            this.charService = charService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CharacterViewModel> models = default;

            try
            {
                models = await this.charService.LeaderboardStats();

                models = models
                    .OrderByDescending(x => x.Rating)
                    .ThenBy(x => x.Name)
                    .Take(3);
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