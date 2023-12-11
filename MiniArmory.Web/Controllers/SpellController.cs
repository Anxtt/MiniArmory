using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Spell;
using MiniArmory.Core.Services.Contracts;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Controllers
{
    public class SpellController : Controller
    {
        private readonly IMemoryCache memoryCache;

        private readonly ISpellService spellService;

        public SpellController(ISpellService spellService, IMemoryCache memoryCache)
        {
            this.spellService = spellService;
            this.memoryCache = memoryCache;
        }

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
                ModelState.AddModelError(nameof(model.Name), Validation.INVALID_NAME);
                return this.View(model);
            }

            try
            {
                await this.spellService.Add(model);
                TempData[Temp.MESSAGE] = Temp.CREATE_SPELL;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AllSpells));
        }

        public async Task<IActionResult> AllSpells(int pageNo = 1, int pageSize = 10)
        {
            SpellListViewModel models = default;

            try
            {
                models = await this.spellService.AllSpells(pageNo, pageSize);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> DeleteSpell([FromQuery]string name)
        {
            try
            {
                await this.spellService.DeleteSpell(name);
                TempData[Temp.MESSAGE] = Temp.DELETE_SPELL;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AllSpells));
        }

        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> EditSpell([FromQuery]string name)
        {
            SpellFormModel model = default;

            try
            {
                model = await this.spellService.FindSpell(name);
                model.Name = name;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> EditSpell(SpellFormModel model)
        {
            try
            {
                await this.spellService.EditSpell(model);
                TempData[Temp.MESSAGE] = string.Format(Temp.EDIT_SPELL, model.Name);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AllSpells));
        }

        public async Task<IActionResult> FilterSpells(string? type = null, int pageNo = 1, int pageSize = 10)
        {
            SpellListViewModel filteredSpells = default;

            try
            {
                if (type != null)
                {
                    filteredSpells = await this.spellService.FilteredSpells(type, pageNo, pageSize);
                }
                else
                {
                    filteredSpells = await this.spellService.AllSpells(pageNo, pageSize);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.PartialView("_AllSpellsPartialView", filteredSpells);
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

        public IActionResult SpellTypes()
            => Json(new string[]
            {
                "Class",
                "Race"
            });
    }
}
