using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Realm;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class RealmController : Controller
    {
        private readonly IRealmService realmService;

        public RealmController(IRealmService realmService)
            => this.realmService = realmService;

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
            IEnumerable<RealmViewModel> realms = default;

            try
            {
                realms = await this.realmService.AllRealms();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

            return this.View(realms);
        }
    }
}
