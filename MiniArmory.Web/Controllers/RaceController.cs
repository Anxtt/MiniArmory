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

        [HttpPost]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> AddRace(RaceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.raceService.DoesExist(model.Name))
            {
                ModelState.AddModelError("Name", "Invalid Name");
                return this.View(model);
            }

            try
            {
                await this.raceService.Add(model);
                TempData["Message"] = "Created race successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AllRaces));
        }

        public async Task<IActionResult> AllRaces()
        {
            IEnumerable<RaceViewModel> races = default;

            try
            {
                races = await this.raceService.AllRaces();
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(races);
        }

        public async Task<IActionResult> Details(int id)
        {
            RaceViewModel race = default;

            try
            {
                race = await this.raceService.GetRace(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(race);
        }

        public async Task<IActionResult> GetRacialSpells()
        {
            var racials = await this.raceService.GetRacialSpells();

            return Json(racials);
        }
    }
}
