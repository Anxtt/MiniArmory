using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class MountController : Controller
    {
        private readonly IMemoryCache memoryCache;

        private readonly IMountService mountService;

        public MountController(IMountService mountService, IMemoryCache memoryCache)
        {
            this.mountService = mountService;
            this.memoryCache = memoryCache;
        }

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
                TempData["Message"] = "Created mount successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AllMounts));
        }

        public async Task<IActionResult> AllMounts()
        {
            IEnumerable<MountViewModel> models = default;
            string cacheKey = "allMountsKey";

            try
            {
                models = this.memoryCache.Get<IEnumerable<MountViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.mountService.AllMounts();

                    var options = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(5)
                    };

                    this.memoryCache.Set(cacheKey, models, options);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
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
