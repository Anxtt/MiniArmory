using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
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
            await this.spellService.Add(model);

            return View(model);
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
