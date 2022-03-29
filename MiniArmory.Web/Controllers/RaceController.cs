using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceService raceService;

        public RaceController(IRaceService raceService) 
            => this.raceService = raceService;

        public IActionResult AddRace()
            => this.View();

        [HttpPost]
        public async Task<IActionResult> AddRace(RaceFormModel model) 
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.raceService.Add(model);
            var races = await this.raceService.AllRaces();

            return this.View("AllRaces", races);
        }

        public async Task<IActionResult> AllRaces()
        {
            var races = await this.raceService.AllRaces();

            return this.View(races);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public async Task<IActionResult> GetRacialSpells()
        {
            var racials = await this.raceService.GetRacialSpells();

            return Json(racials);
        }
    }
}
