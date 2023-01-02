using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniArmory.Core.Models.Class;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IMemoryCache memoryCache;

        private readonly IClassService classService;

        public ClassController(IClassService classService, IMemoryCache memoryCache)
        {
            this.classService = classService;
            this.memoryCache = memoryCache;
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
                ModelState.AddModelError("Name", "Invalid Name");
                return this.View(model);
            }

            try
            {
                await this.classService.Add(model);
                TempData["Message"] = "Created class successfully.";
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(AllClasses));
        }

        public async Task<IActionResult> AllClasses()
        {
            IEnumerable<ClassViewModel> models = default;
            string cacheKey = "allClassesKey";

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
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(models);
        }

        public async Task<IActionResult> Details(int id)
        {
            ClassViewModel model = default;

            if (!await this.classService.DoesExist(id))
            {
                return this.RedirectToAction("Error", "Home");
            }

            try
            {
                model = await this.classService.Details(id);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
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
