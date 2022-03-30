using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
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

            await this.factionService.Add(model);

            return this.View();
        }
    }
}
