using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService classService;

        public ClassController(IClassService classService) 
            => this.classService = classService;

        [HttpGet]
        public IActionResult AddClass() 
            => View();

        [HttpPost]
        public IActionResult AddClass(ClassFormModel model)
        {
            this.classService.Add(model);

            return this.View();
        }
    }
}
