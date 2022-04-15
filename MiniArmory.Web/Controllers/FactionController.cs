using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Faction;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class FactionController : Controller
    {
        private readonly IFactionService factionService;

        public FactionController(IFactionService factionService)
            => this.factionService = factionService;

        [Authorize(Roles = "Owner")]
        public IActionResult AddFaction()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> AddFaction(FactionFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.factionService.DoesExist(model.Name))
            {
                ModelState.AddModelError("Name", "Invalid Name");
                return this.View(model);
            }

            try
            {
                await this.factionService.Add(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "HomeController");
            }

            return this.RedirectToAction("AllRaces", "Race");
        }
    }
}
