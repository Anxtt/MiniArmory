﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MiniArmory.Core.Models.Race;
using MiniArmory.Core.Services.Contracts;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Controllers
{
    public class RaceController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IRedisService redis;

        private readonly IRaceService raceService;

        public RaceController(IRaceService raceService, IMemoryCache memoryCache, IRedisService redis)
        {
            this.raceService = raceService;
            this.memoryCache = memoryCache;
            this.redis = redis;
        }

        [Authorize(Roles = "Owner, Admin")]
        public IActionResult AddRace()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> AddRace(RaceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.raceService.DoesExist(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), Validation.INVALID_NAME);
                return this.View(model);
            }

            try
            {
                await this.raceService.Add(model);
                TempData[Temp.MESSAGE] = Temp.CREATE_RACE;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AllRaces));
        }

        public async Task<IActionResult> AllRaces()
        {
            IEnumerable<RaceViewModel> models = default;
            string cacheKey = Cache.ALL_RACES_KEY;

            try
            {
                models = this.memoryCache.Get<IEnumerable<RaceViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.raceService.AllRaces();

                    var options = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddDays(1)
                    };

                    this.memoryCache.Set(cacheKey, models, options);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(models);
        }

        public async Task<IActionResult> Details(int id)
        {
            RaceViewModel model = default;
            string cacheKey = string.Format(RedisCache.DETAILS_RACE_KEY, id);

            try
            {
                model = await this.redis.RetrieveCache<RaceViewModel>(cacheKey);

                if (model == null)
                {
                    model = await this.raceService.GetRace(id);

                    await this.redis.SetCache(cacheKey, model);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(model);
        }

        public async Task<IActionResult> GetRacialSpells()
        {
            var racials = await this.raceService.GetRacialSpells();

            return Json(racials);
        }
    }
}
