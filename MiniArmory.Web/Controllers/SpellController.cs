using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "Owner, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSpell(SpellFormModel model)
        {
            if (!ModelState.IsValid)
            {
                if (await this.spellService.DoesExist(model.Name))
                {
                    ModelState.AddModelError(nameof(model.Name), "Invalid name.");
                }

                return this.View(model);
            }

            await this.spellService.Add(model);

            return this.RedirectToAction(nameof(AllSpells));
        }

        public async Task<IActionResult> AllSpells()
        {
            var spells = await this.spellService.AllSpells();

            return this.View(spells);
        }

        [IgnoreAntiforgeryToken]
        public IActionResult SpellTypes()
        {
            string[] types = new string[]
            {
                "Class",
                "Race"
            };

            return Json(types);
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await this.spellService.GetClasses();

            return Json(classes);
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetRaces()
        {
            var races = await this.spellService.GetRaces();

            return Json(races);
        }
    }
}
