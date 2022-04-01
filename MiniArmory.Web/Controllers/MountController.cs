using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class MountController : Controller
    {
        private readonly IMountService mountService;

        public MountController(IMountService mountService) 
            => this.mountService = mountService;

        public IActionResult AddMount() 
            => this.View(); 
        
        [HttpPost]
        public async Task<IActionResult> AddMount(MountFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.mountService.Add(model);
            var mounts = await this.mountService.AllMounts();

            return this.RedirectToAction(nameof(AllMounts));
        }
        
        public async Task<IActionResult> AllMounts()
        {
            var mounts = await this.mountService.AllMounts();

            return this.View(mounts);
        }
        
        public async Task<IActionResult> GetFactions()
        {
            var factions = await this.mountService.GetFactions();

            return Json(factions);
        }
    }
}
