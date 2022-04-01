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

        [HttpGet]
        public IActionResult AddSpell()
            => this.View();

        [HttpPost]
        public async Task<IActionResult> AddSpell(SpellFormModel model)
        {
            if (!ModelState.IsValid)
            {
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

        public async Task<IActionResult> GetRaces()
        {
            var races = await this.spellService.GetRaces();

            return Json(races);
        }
    }
}
