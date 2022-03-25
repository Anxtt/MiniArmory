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

        public IActionResult AddFaction()
            => this.View();

        [HttpPost]
        public async Task<IActionResult> AddFaction(FactionFormModel model)
        {
            await this.factionService.Add(model);

            return this.View();
        }
    }
}
