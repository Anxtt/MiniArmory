using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class MountController : Controller
    {
        private readonly IMountService mountService;

        public MountController(IMountService mountService)
            => this.mountService = mountService;

        [Authorize(Roles = "Owner, Admin")]
        public IActionResult AddMount()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> AddMount(MountFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.mountService.DoesExist(model.Name))
            {
                ModelState.AddModelError("Name", "Invalid Name");
                return this.View(model);
            }

            try
            {
                await this.mountService.Add(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.RedirectToAction(nameof(AllMounts));
        }

        public async Task<IActionResult> AllMounts()
        {
            IEnumerable<MountViewModel> mounts = default;

            try
            {
                mounts = await this.mountService.AllMounts();
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.View(mounts);
        }

        public async Task<IActionResult> GetFactions(int? factionId = null)
        {
            IEnumerable<JsonFormModel> factions = default;

            if (factionId != null)
            {
                factions = await this.mountService.GetSpecificFaction(factionId);
            }
            else
            {
                factions = await this.mountService.GetFactions();
            }

            return Json(factions);
        }
    }
}
