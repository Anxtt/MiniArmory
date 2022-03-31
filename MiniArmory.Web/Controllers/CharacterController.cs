using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Web.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ICharacterService charService;
        private readonly UserManager<User> userManager;

        public CharacterController(ICharacterService charService,
            UserManager<User> userManager)
        {
            this.charService = charService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult AddCharacter()
            => this.View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCharacter(CharacterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await userManager.FindByNameAsync(this.User.Identity.Name);

            await this.charService.Add(model, user.Id);

            return this.RedirectToAction(nameof(CharacterDetails));
        }

        [Authorize]
        public async Task<IActionResult> CharacterDetails()
        {
            return this.View();
        }

        public async Task<IActionResult> Search(string chars)
        {
            if (string.IsNullOrWhiteSpace(chars))
            {
                return this.View();
            }

            var characters = await this.charService.SearchCharacters(chars);

            return this.View();
        }

        public async Task<IActionResult> Leaderboard()
        {
            var chars = await this.charService.LeaderboardStats();

            return this.View(chars);
        }

        public async Task<IActionResult> GetRealms()
        {
            var realms = await this.charService.GetRealms();

            return Json(realms);
        }
    }
}
