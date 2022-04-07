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

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> AddRealm(RealmFormModel model)
        {
            if (!ModelState.IsValid)
            {
                if (await this.realmService.DoesExist(model.Name))
                {
                    ModelState.AddModelError("Name", "Invalid Name");
                }

                return this.View(model);
            }

            await this.realmService.Add(model);

            return this.RedirectToAction(nameof(AllRealms));
        }

        public async Task<IActionResult> AllRealms()
        {
            var realms = await this.realmService.AllRealms();

            return this.View(realms);
        }
    }
}
