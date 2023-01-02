using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Web.Controllers
{
    public class CharacterController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMemoryCache memoryCache;

        private readonly ICharacterService charService;
        private readonly IMountService mountService;

        public CharacterController(UserManager<User> userManager,
            ICharacterService charService,
            IMountService mountService,
            IMemoryCache memoryCache)
        {
            this.userManager = userManager;
            this.charService = charService;
            this.mountService = mountService;
            this.memoryCache = memoryCache;
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public IActionResult AddCharacter()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddCharacter(CharacterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.charService.DoesExist(model.Name))
            {
                ModelState.AddModelError("Name", "Invalid Name");
                return this.View(model);
            }

            try
            {
                var user = await userManager.FindByNameAsync(this.User.Identity.Name);

                await this.charService.Add(model, user.Id);
                TempData["Message"] = "Created character successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(CharacterList));
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> CharacterList()
        {
            User user = default;
            IEnumerable<CharacterViewModel> models = default;

            try
            {
                user = await userManager.FindByNameAsync(this.User.Identity.Name);
                models = await this.charService.OwnCharacters(user.Id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddMount(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction("Error", "Home");
            }

            IEnumerable<MountViewModel> mounts = default;
            CharacterViewModel character = default;

            try
            {
                mounts = await this.charService.UnownedMounts(id);
                character = await this.charService.FindCharacterById(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            MountCharacterViewModel model = new MountCharacterViewModel()
            {
                Character = character,
                Mounts = mounts
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddMount(Guid id, string mountName)
        {
            if (id == default(Guid) ||
                !await this.charService.DoesExist(id) ||
                string.IsNullOrEmpty(mountName))
            {
                return this.RedirectToAction(nameof(AddMount), new { id = id });
            }

            if (this.charService.RollForReward("Mount") == false)
            {
                return this.RedirectToAction(nameof(AddMount), new { id = id });
            }

            try
            {
                await this.charService.AddMountToCharacter(id, mountName);
                TempData["Message"] = "Added mount successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AddMount), new { id = id });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddAchievement(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction("Error", "Home");
            }

            IEnumerable<AchievementViewModel> achies = default;
            CharacterViewModel character = default;

            try
            {
                achies = await this.charService.UnownedAchievements(id);
                character = await this.charService.FindCharacterById(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            AchievementCharacterViewModel model = new AchievementCharacterViewModel()
            {
                Character = character,
                Achievements = achies
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddAchievement(Guid id, string achievement)
        {
            if (id == default(Guid) ||
                !await this.charService.DoesExist(id) ||
                string.IsNullOrEmpty(achievement))
            {
                return this.RedirectToAction("Error", "Home");
            }

            if (this.charService.RollForReward("Achievement") == false)
            {
                return this.RedirectToAction(nameof(AddAchievement), new { id = id });
            }

            try
            {
                await this.charService.AddAchievementToCharacter(id, achievement);
                TempData["Message"] = "Added achievement successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AddAchievement), new { id = id });
        }

        public async Task<IActionResult> Achievements(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction("Error", "Home");
            }

            IEnumerable<AchievementViewModel> achievements = default;
            CharacterViewModel character = default;

            try
            {
                achievements = await this.charService.OwnAchievements(id);
                character = await this.charService.FindCharacterById(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

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
            CharacterFormModel model = default;

            try
            {
                model = await this.charService.GetCharacterForChange(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeFaction(Guid id, string faction, string race)
        {
            try
            {
                await this.charService.ChangeFaction(id, faction);
                await this.charService.ChangeRace(id, race);
                TempData["Message"] = "Changed race and faction successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(Details), new { id = id });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeName(Guid id)
        {
            CharacterFormModel model = default;

            try
            {
                model = await this.charService.GetCharacterForChange(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeName(Guid id, string name)
        {
            try
            {
                await this.charService.ChangeName(id, name);
                TempData["Message"] = "Changed name successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(Details), new { id = id });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeRace(Guid id)
        {
            CharacterFormModel model = default;

            try
            {
                model = await this.charService.GetCharacterForChange(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> ChangeRace(Guid id, string race)
        {
            try
            {
                await this.charService.ChangeRace(id, race);
                TempData["Message"] = "Changed race successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> DeleteCharacter(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(CharacterList));
            }

            try
            {
                await this.charService.Delete(id);
                TempData["Message"] = "Deleted character successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(CharacterList));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction("Error", "Home");
            }

            CharacterViewModel model = default;

            try
            {
                model = await this.charService.FindCharacterById(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(model);
        }

        public async Task<IActionResult> Leaderboard()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = "leaderboardKey";

            try
            {
                models = this.memoryCache.Get<IEnumerable<CharacterViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.charService.LeaderboardStats();

                    var options = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(10)
                    };

                    this.memoryCache.Set(cacheKey, models, options);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> LFG(Guid id)
        {
            LFGFormModel model = default;

            try
            {
                model = await this.charService.LFGCharacter(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> Mounts(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction("Error", "Home");
            }

            IEnumerable<MountViewModel> mounts = default;
            CharacterViewModel character = default;

            try
            {
                mounts = await this.charService.OwnMounts(id);
                character = await this.charService.FindCharacterById(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

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
                return this.RedirectToAction("Error", "Home");
            }

            try
            {
                await this.charService.EarnRating(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(PlayArena));
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> PlayArena()
        {
            User user = default;
            IEnumerable<CharacterViewModel> models = default;

            try
            {
                user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
                models = await this.charService.OwnCharacters(user.Id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> EarnRatingAsTeam(Guid id, Guid partnerId)
        {
            try
            {
                await this.charService.EarnRatingAsTeam(id, partnerId);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> LeaveTeam(Guid id, Guid partnerId)
        {
            try
            {
                await this.charService.LeaveTeam(id, partnerId);
                TempData["Message"] = "You left your team.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(CharacterList));
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> TeamUp(Guid id, Guid partnerId)
        {
            try
            {
                await this.charService.TeamUp(id, partnerId);
                TempData["Message"] = "You are in a team now.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> PlayArenaAsTeam(Guid id, Guid partnerId)
        {
            CharacterViewModel character = default;
            CharacterViewModel partner = default;

            try
            {
                character = await this.charService.FindCharacterById(id);
                partner = await this.charService.FindCharacterById(partnerId);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            List<CharacterViewModel> models = new List<CharacterViewModel>();

            models.Add(character);
            models.Add(partner);

            return this.View(models);
        }

        public async Task<IActionResult> Ranking()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = "achievementKey";

            try
            {
                models = this.memoryCache.Get<IEnumerable<CharacterViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.charService.AchievementStats();

                    var options = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(10)
                    };

                    this.memoryCache.Set(cacheKey, models, options);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string chars)
        {
            if (string.IsNullOrWhiteSpace(chars))
            {
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<CharacterViewModel> models = default;

            try
            {
                models = await this.charService.SearchCharacters(chars);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

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

            try
            {
                await this.charService.SignUp(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(LFG), new { id = model.Id });
        }

        public async Task<IActionResult> GetRealms()
        {
            var realms = await this.charService.GetRealms();

            return Json(realms);
        }
    }
}