using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Web.Extensions;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Controllers
{
    public class CharacterController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IRedisService redis;

        private readonly ICharacterService charService;
        private readonly IMountService mountService;

        public CharacterController(
            ICharacterService charService,
            IMountService mountService,
            IMemoryCache memoryCache,
            IRedisService redis)
        {
            this.charService = charService;
            this.mountService = mountService;
            this.memoryCache = memoryCache;
            this.redis = redis;
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
                ModelState.AddModelError(nameof(model.Name), Validation.INVALID_NAME);
                return this.View(model);
            }

            try
            {
                Guid userId = this.User.GetId();

                await this.charService.Add(model, userId);
                TempData[Temp.MESSAGE] = Temp.CREATE_CHARACTER;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(CharacterList));
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddMount(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            if (this.charService.RollForReward(If.MOUNT) == false)
            {
                return this.RedirectToAction(nameof(AddMount), new { id = id });
            }

            try
            {
                await this.charService.AddMountToCharacter(id, mountName);
                TempData[Temp.MESSAGE] = Temp.ADDED_MOUNT;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AddMount), new { id = id });
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> AddAchievement(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            if (this.charService.RollForReward(If.ACHIEVEMENT) == false)
            {
                return this.RedirectToAction(nameof(AddAchievement), new { id = id });
            }

            try
            {
                await this.charService.AddAchievementToCharacter(id, achievement);
                TempData[Temp.MESSAGE] = Temp.ADDED_ACHIEVEMENT;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AddAchievement), new { id = id });
        }

        public async Task<IActionResult> Achievements(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            AchievementCharacterViewModel model = new AchievementCharacterViewModel()
            {
                Achievements = achievements,
                Character = character
            };

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> CharacterList()
        {
            Guid userId = this.User.GetId();
            IEnumerable<CharacterViewModel> models = default;

            try
            {
                models = await this.charService.OwnCharacters(userId);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                TempData[Temp.MESSAGE] = Temp.CHANGE_FACTION;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                TempData[Temp.MESSAGE] = Temp.CHANGE_NAME;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                TempData[Temp.MESSAGE] = Temp.CHANGE_RACE;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                TempData[Temp.MESSAGE] = Temp.DELETED;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(CharacterList));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == default || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            CharacterViewModel model = default;
            string cacheKey = string.Format(RedisCache.DETAILS_CHARACTER_KEY, id);

            try
            {
                model = await this.redis.RetrieveCache<CharacterViewModel>(cacheKey);

                if (model == null)
                {
                    model = await this.charService.FindCharacterById(id);

                    await this.redis.SetCache(cacheKey, model);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(model);
        }

        public async Task<IActionResult> Leaderboard()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = Cache.LEADERBOARD_KEY;

            try
            {
                models = await this.memoryCache.GetOrCreateAsync(cacheKey, async x =>
                {
                    x.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
                    x.Priority = CacheItemPriority.High;
                    x.SlidingExpiration = TimeSpan.FromSeconds(10);

                    return await this.charService.LeaderboardStats();
                });
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> LeaveTeam(Guid id, Guid partnerId)
        {
            try
            {
                await this.charService.LeaveTeam(id, partnerId);
                TempData[Temp.MESSAGE] = Temp.LEAVE_TEAM;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(CharacterList));
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> Mounts(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            MountCharacterViewModel model = new MountCharacterViewModel()
            {
                Mounts = mounts,
                Character = character
            };

            return this.View(model);
        }

        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> PlayArena()
        {
            Guid userId = this.User.GetId();
            IEnumerable<CharacterViewModel> models = default;

            try
            {
                models = await this.charService.OwnCharacters(userId);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> EarnRating(Guid id)
        {
            if (id == default(Guid) || !await this.charService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            try
            {
                await this.charService.EarnRating(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(PlayArena));
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            List<CharacterViewModel> models = new List<CharacterViewModel>
            {
                character,
                partner
            };

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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> EarnRatingAsTeamVsTeam(Guid id, Guid partnerId)
        {
            string status;

            try
            {
                (string enemy, string enemyPartner, status) = await this.charService
                    .EarnRatingAsTeamVsTeam(id, partnerId);

                TempData[Temp.MESSAGE] = status;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        public async Task<IActionResult> Ranking()
        {
            IEnumerable<CharacterViewModel> models = default;
            string cacheKey = Cache.ACHIEVEMENT_KEY;

            try
            {
                models = await this.memoryCache.GetOrCreateAsync(cacheKey, async x =>
                {
                    x.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
                    x.Priority = CacheItemPriority.High;
                    x.SlidingExpiration = TimeSpan.FromSeconds(10);

                    return await this.charService.AchievementStats();
                });
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string chars)
        {
            if (string.IsNullOrWhiteSpace(chars))
            {
                return this.RedirectToAction(nameof(HomeController.Index), ControllerConst.HOME);
            }

            IEnumerable<CharacterViewModel> models = default;

            try
            {
                models = await this.charService.SearchCharacters(chars);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
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
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(LFG), new { id = model.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin, Owner")]
        public async Task<IActionResult> TeamUp(Guid id, Guid partnerId)
        {
            try
            {
                await this.charService.TeamUp(id, partnerId);
                TempData[Temp.MESSAGE] = Temp.TEAM_UP;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(PlayArenaAsTeam), new { id = id, partnerId = partnerId });
        }

        public async Task<IActionResult> GetRealms()
        {
            var realms = await this.charService.GetRealms();

            return Json(realms);
        }
    }
}