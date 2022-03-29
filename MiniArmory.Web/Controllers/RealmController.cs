using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class RealmController : Controller
    {
        private readonly IRealmService realmService;

        public RealmController(IRealmService realmService)
            => this.realmService = realmService;

        public IActionResult AddRealm() 
            => this.View();

        [HttpPost]
        public async Task<IActionResult> AddRealm(RealmFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.realmService.Add(model);

            return this.View();
        }
    }
}
