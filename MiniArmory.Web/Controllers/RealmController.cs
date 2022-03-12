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

        [HttpGet]
        public IActionResult AddRealm() 
            => View();

        [HttpPost]
        public IActionResult AddRealm(RealmFormModel model)
        {
            this.realmService.Add(model);

            return View();
        }
    }
}
