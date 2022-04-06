using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Race;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceService raceService;

        public RaceController(IRaceService raceService) 
            => this.raceService = raceService;

        [Authorize(Roles = "Owner, Admin")]
        public IActionResult AddRace()
            => this.View();

        [Authorize(Roles = "Owner, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRace(RaceFormModel model) 
        {
            if (!ModelState.IsValid || await this.raceService.DoesExist(model.Name))
            {
                return this.View(model);
            }

            await this.raceService.Add(model);

            return this.RedirectToAction(nameof(AllRaces));
        }

        public async Task<IActionResult> AllRaces()
        {
            var races = await this.raceService.AllRaces();

            return this.View(races);
        }

        public async Task<IActionResult> Details(int id)
        {
            var race = await this.raceService.GetRace(id);

            return this.View(race);
        }

        public async Task<IActionResult> GetRacialSpells()
        {
            var racials = await this.raceService.GetRacialSpells();

            return Json(racials);
        }
    }
}
