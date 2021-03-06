using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Spell;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class SpellController : Controller
    {
        private readonly ISpellService spellService;

        public SpellController(ISpellService spellService)
            => this.spellService = spellService;

        [Authorize(Roles = "Owner, Admin")]
        public IActionResult AddSpell()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> AddSpell(SpellFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.spellService.DoesExist(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Invalid name.");
                return this.View(model);
            }

            try
            {
                await this.spellService.Add(model);
                TempData["Message"] = "Created spell successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.RedirectToAction(nameof(AllSpells));
        }

        public async Task<IActionResult> AllSpells()
        {
            IEnumerable<SpellViewModel> spells = default;

            try
            {
                spells = await this.spellService.AllSpells();
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.View(spells);
        }

        public IActionResult SpellTypes()
        {
            string[] types = new string[]
            {
                "Class",
                "Race"
            };

            return Json(types);
        }

        public async Task<IActionResult> GetClasses()
        {
            var classes = await this.spellService.GetClasses();

            return Json(classes);
        }

        public async Task<IActionResult> GetRaces(int? raceId = null, int? factionId = null)
        {
            IEnumerable<JsonFormModel> races = default;

            if (raceId != null && factionId != null)
            {
                races = await this.spellService.GetSameFactionRaces(raceId, factionId);
            }
            else if (factionId != null)
            {
                races = await this.spellService.GetOppositeFactionRaces(factionId);
            }
            else
            {
                races = await this.spellService.GetRaces();
            }

            return Json(races);
        }

        public async Task<IActionResult> FilterSpells(string? type = null)
        {
            IEnumerable<SpellViewModel> filteredSpells = default;

            try
            {
                if (type != null)
                {
                    filteredSpells = await this.spellService.FilteredSpells(type);
                }
                else
                {
                    filteredSpells = await this.spellService.AllSpells();
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.PartialView("_AllSpellsPartialView", filteredSpells);
        }
    }
}
