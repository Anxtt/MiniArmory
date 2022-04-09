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

        [Authorize(Roles = "Owner, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClass(ClassFormModel model)
        {
            if (!ModelState.IsValid)
            {
                if (await this.classService.DoesExist(model.Name))
                {
                    ModelState.AddModelError("Name", "Invalid Name");
                }

                return this.View(model);
            }

            await this.classService.Add(model);

            return this.RedirectToAction(nameof(AllClasses));
        }

        public async Task<IActionResult> AllClasses()
        {
            var classes = await this.classService.AllClasses();

            return this.View(classes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await this.classService.Details(id);

            return this.View(model);
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetSpells()
        {
            var models = await this.classService.GetSpells();

            return Json(models);
        }
    }
}
