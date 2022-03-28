using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
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
            await this.mountService.Add(model);

            return this.View();
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
