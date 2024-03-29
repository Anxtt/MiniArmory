﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MiniArmory.Core.Models.Class;
using MiniArmory.Core.Services.Contracts;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IRedisService redis;

        private readonly IClassService classService;

        public ClassController(IClassService classService, IMemoryCache memoryCache, IRedisService redis)
        {
            this.classService = classService;
            this.memoryCache = memoryCache;
            this.redis = redis;
        }

        [Authorize(Roles = "Owner, Admin")]
        public IActionResult AddClass()
            => this.View();

        [HttpPost]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> AddClass(ClassFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (await this.classService.DoesExist(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), Validation.INVALID_NAME);
                return this.View(model);
            }

            try
            {
                await this.classService.Add(model);
                TempData[Temp.MESSAGE] = Temp.CREATE_CLASS;
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.RedirectToAction(nameof(AllClasses));
        }

        public async Task<IActionResult> AllClasses()
        {
            IEnumerable<ClassViewModel> models = default;
            string cacheKey = Cache.ALL_CLASSES_KEY;

            try
            {
                models = this.memoryCache.Get<IEnumerable<ClassViewModel>>(cacheKey);

                if (models == null)
                {
                    models = await this.classService.AllClasses();

                    var options = new MemoryCacheEntryOptions()
                    {
                        Priority = CacheItemPriority.Normal,
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
            ClassViewModel model = default;
            string cacheKey = string.Format(RedisCache.DETAILS_CLASS_KEY, id);

            if (!await this.classService.DoesExist(id))
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            try
            {
                model = await this.redis.RetrieveCache<ClassViewModel>(cacheKey);

                if (model == null)
                {
                    model = await this.classService.Details(id);

                    await this.redis.SetCache(cacheKey, model);
                }
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(HomeController.Error), ControllerConst.HOME);
            }

            return this.View(model);
        }

        public async Task<IActionResult> GetSpells()
        {
            var models = await this.classService.GetSpells();

            return Json(models);
        }
    }
}
