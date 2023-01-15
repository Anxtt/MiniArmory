using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MiniArmory.Core.Models.Realm;
using MiniArmory.Core.Services.Contracts;

using static MiniArmory.GlobalConstants.Web;

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
                ModelState.AddModelError(nameof(model.Name), Validation.INVALID_NAME);
                return this.View(model);
            }

            try
            {
                await this.realmService.Add(model);
                TempData[Temp.MESSAGE] = Temp.CREATE_REALM;
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AllRealms));
        }

        public async Task<IActionResult> AllRealms()
        {
            IEnumerable<RealmViewModel> models = default;
            string cacheKey = Cache.ALL_REALMS_KEY;

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
                return RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
        }
    }
}
