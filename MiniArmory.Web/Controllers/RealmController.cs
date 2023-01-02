using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models.Realm;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class RealmController : Controller
    {
        private readonly IMemoryCache memoryCache;

        private readonly IRealmService realmService;

        public RealmController(IRealmService realmService, IMemoryCache memoryCache)
        {
            this.realmService = realmService;
            this.memoryCache = memoryCache;
        }

        [Authorize(Roles = "Owner")]
        public IActionResult AddRealm()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> AddRealm(RealmFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.realmService.DoesExist(model.Name))
            {
                ModelState.AddModelError("Name", "Invalid Name");
                return this.View(model);
            }

            try
            {
                await this.realmService.Add(model);
                TempData["Message"] = "Created realm successfully.";
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AllRealms));
        }

        public async Task<IActionResult> AllRealms()
        {
            IEnumerable<RealmViewModel> models = default;
            string cacheKey = "allRealmsKey";

            try
            {
                models = this.memoryCache.Get<IEnumerable<RealmViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.realmService.AllRealms();

                    var options = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(1)
                    };

                    this.memoryCache.Set(cacheKey, models, options);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }
    }
}
