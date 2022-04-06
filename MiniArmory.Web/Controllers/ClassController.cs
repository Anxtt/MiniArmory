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
            if (!ModelState.IsValid || await this.classService.DoesExist(model.Name))
            {
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

        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> AddSpells(int id)
        {
            var classEntity = await this.classService.GetClass(id);

            return this.View(classEntity);
        }

        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> AddSpells(ClassViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return this.View(model);
            //}

            await this.classService.AddSpells(model);

            return this.Redirect(nameof(Details));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = 2;

            return this.View(model);
        }

        public async Task<IActionResult> GetSpells()
        {
            var models = await this.classService.GetSpells();

            return Json(models);
        }
    }
}
