﻿using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService classService;

        public ClassController(IClassService classService) 
            => this.classService = classService;

        public IActionResult AddClass() 
            => this.View();

        [HttpPost]
        public async Task<IActionResult> AddClass(ClassFormModel model)
        {
            await this.classService.Add(model);

            return this.View();
        }

        public async Task<IActionResult> AllClasses()
        {
            var classes = await this.classService.AllClasses();

            return this.View(classes);
        }
    }
}
