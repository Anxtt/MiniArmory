using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Character;
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

            return this.RedirectToAction(nameof(Details));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return this.View(id);
            }

            var model = await this.charService.FindCharacterById(id);

            return this.View(model);
        }

        public async Task<IActionResult> Leaderboard()
        {
            var models = await this.charService.LeaderboardStats();

            return this.View(models);
        }

        public async Task<IActionResult> PlayArena()
        {
            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            var models = await this.charService.OwnCharacters(user.Id);

            return this.View(models);
        }

        public async Task<IActionResult> Ranking()
        {
            var models = await this.charService.AchievementStats();

            return this.View(models);
        }

        public async Task<IActionResult> Search(string chars)
        {
            if (string.IsNullOrWhiteSpace(chars))
            {
                return this.View();
            }

            var models = await this.charService.SearchCharacters(chars);

            return this.View(models);
        }

        public async Task<IActionResult> GetRealms()
        {
            var realms = await this.charService.GetRealms();

            return Json(realms);
        }
    }
}