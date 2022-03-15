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
        public IActionResult AddSpell(SpellFormModel model)
        {
            this.spellService.Add(model);

            return View();
        }

        public IActionResult GetClasses()
        {
            var classes = this.spellService.GetClasses();

            return Json(classes);
        }

        public IActionResult GetRaces()
        {
            var races = this.spellService.GetRaces();

            return Json(races);
        }
    }
}
