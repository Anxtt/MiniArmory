using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Class;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService classService;

        public ClassController(IClassService classService)
            => this.classService = classService;

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
            IEnumerable<ClassViewModel> classes = default;

            try
            {
                classes = await this.classService.AllClasses();
            }
            catch (Exception)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(classes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new ClassViewModel();

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
