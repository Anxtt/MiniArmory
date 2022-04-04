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
        private readonly UserManager<User> userManager;

        private readonly ICharacterService charService;
        private readonly IMountService mountService;

        public CharacterController(UserManager<User> userManager, 
            ICharacterService charService,
            IMountService mountService)
        {
            this.userManager = userManager;
            this.charService = charService;
            this.mountService = mountService;
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

        public async Task<IActionResult> CharacterList()
        {
            var user = await userManager.FindByNameAsync(this.User.Identity.Name);

            var models = await this.charService.OwnCharacters(user.Id);

            return this.View(models);
        }

        public async Task<IActionResult> AddMount(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            var mounts = await this.charService.UnownedMounts(id);
            var character = await this.charService.FindCharacterById(id);

            MountCharacterViewModel model = new MountCharacterViewModel()
            {
                Character = character,
                Mounts = mounts
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMount(Guid id, string mountName)
        {
            if (id == default(Guid) ||
                !await this.charService.DoesExist(id) ||
                string.IsNullOrEmpty(mountName))
            {
                return this.View();
            }

            if (this.charService.RollForReward("Mount") == false)
            {
                return this.RedirectToAction(nameof(AddMount), id);
            }

            await this.charService.AddMountToCharacter(id, mountName);

            return this.RedirectToAction(nameof(AddMount), id);
        }

        public async Task<IActionResult> AddAchievement(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            var achies = await this.charService.UnownedAchievements(id);
            var character = await this.charService.FindCharacterById(id);

            AchievementCharacterViewModel model = new AchievementCharacterViewModel()
            {
                Character = character,
                Achievements = achies
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAchievement(Guid id, string achievement)
        {
            if (id == default(Guid) ||
                !await this.charService.DoesExist(id) ||
                string.IsNullOrEmpty(achievement))
            { 
                return this.View();
            }

            if (this.charService.RollForReward("Achievement") == false)
            {
                return this.RedirectToAction(nameof(AddAchievement), id);
            }

            await this.charService.AddAchievementToCharacter(id, achievement);

            return this.RedirectToAction(nameof(AddAchievement), id);
        }

        public async Task<IActionResult> Achievements(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            var achievements = await this.charService.OwnAchievements(id);
            var character = await this.charService.FindCharacterById(id);

            AchievementCharacterViewModel model = new AchievementCharacterViewModel()
            {
                Achievements = achievements,
                Character = character
            };

            return this.View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            var model = await this.charService.FindCharacterById(id);

            return this.View(model);
        }

        public async Task<IActionResult> Leaderboard()
        {
            var models = await this.charService.LeaderboardStats();

            return this.View(models);
        }

        [HttpPost]
        public async Task<IActionResult> EarnRating(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            await this.charService.EarnRating(id);

            return this.RedirectToAction(nameof(PlayArena));
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

        [HttpPost]
        public async Task<IActionResult> SignUp(LFGFormModel model)
        {
            await this.charService.SignUp(model);

            return this.View(model);
        }

        public async Task<IActionResult> GetRealms()
        {
            var realms = await this.charService.GetRealms();

            return Json(realms);
        }
    }
}