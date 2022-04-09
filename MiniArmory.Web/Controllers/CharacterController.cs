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

        [Authorize(Roles = "Member, Admin, Owner")]
        public IActionResult AddCharacter()
            => this.View();

        [Authorize(Roles = "Member, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> AddCharacter(CharacterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                if (await this.charService.DoesExist(model.Name))
                {
                    ModelState.AddModelError("Name", "Invalid Name");
                }

                return this.View(model);
            }

            var user = await userManager.FindByNameAsync(this.User.Identity.Name);

            await this.charService.Add(model, user.Id);

            return this.RedirectToAction(nameof(Details));
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> CharacterList()
        {
            var user = await userManager.FindByNameAsync(this.User.Identity.Name);

            var models = await this.charService.OwnCharacters(user.Id);

            return this.View(models);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
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

        [Authorize(Roles = "Member, Admin, Owner")]
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

        [Authorize(Roles = "Member, Admin, Owner")]
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

        [Authorize(Roles = "Member, Admin, Owner")]
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

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeFaction(Guid id)
        {
            var model = await this.charService.GetCharacterForChange(id);

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> ChangeFaction(Guid id, string faction)
        {
            await this.charService.ChangeFaction(id, faction);

            return this.RedirectToAction(nameof(Details), id);
        }
        
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeName(Guid id)
        {
            var model = await this.charService.GetCharacterForChange(id);

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> ChangeName(Guid id, string name)
        {
            await this.charService.ChangeName(id, name);

            return this.RedirectToAction(nameof(Details), id);
        }
        
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeRace(Guid id)
        {
            var model = await this.charService.GetCharacterForChange(id);

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> ChangeRace(Guid id, string race)
        {
            await this.charService.ChangeRace(id, race);

            return this.RedirectToAction(nameof(Details), id);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> DeleteCharacter(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(CharacterList));
            }

            await this.charService.Delete(id);

            return this.RedirectToAction(nameof(CharacterList));
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

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> LFG(Guid id)
        {
            var model = await this.charService.LFGCharacter(id);

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> Mounts(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            var mounts = await this.charService.OwnMounts(id);
            var character = await this.charService.FindCharacterById(id);

            MountCharacterViewModel model = new MountCharacterViewModel()
            {
                Mounts = mounts,
                Character = character
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> EarnRating(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.View();
            }

            await this.charService.EarnRating(id);

            return this.RedirectToAction(nameof(PlayArena));
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> PlayArena()
        {
            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            var models = await this.charService.OwnCharacters(user.Id);

            return this.View(models);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> EarnRatingAsTeam(Guid id, Guid partnerId)
        {
            await this.charService.EarnRatingAsTeam(id, partnerId);

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> LeaveTeam(Guid id, Guid partnerId)
        {
            await this.charService.LeaveTeam(id, partnerId);

            return this.RedirectToAction(nameof(CharacterList));
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> TeamUp(Guid id, Guid partnerId)
        {
            await this.charService.TeamUp(id, partnerId);

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> PlayArenaAsTeam(Guid id, Guid partnerId)
        {
            var character = await this.charService.FindCharacterById(id);
            var partner = await this.charService.FindCharacterById(partnerId);

            List<CharacterViewModel> models = new List<CharacterViewModel>();

            models.Add(character);
            models.Add(partner);

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

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> SignUp(LFGFormModel model)
        {
            var (isLooking, partnerId) = await this.charService.IsLooking(model.Id);

            if (partnerId != null)
            {
                return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = model.Id, partnerId = partnerId });
            }

            if (isLooking)
            {
                return this.RedirectToAction(nameof(LFG), new { id = model.Id });
            }

            await this.charService.SignUp(model);

            return this.RedirectToAction(nameof(LFG), new { id = model.Id });
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetRealms()
        {
            var realms = await this.charService.GetRealms();

            return Json(realms);
        }
    }
}