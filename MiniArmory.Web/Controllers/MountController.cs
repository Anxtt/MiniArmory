using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Owner, Admin")]
        public IActionResult AddMount() 
            => this.View(); 
        
        [Authorize(Roles = "Owner, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMount(MountFormModel model)
        {
            if (!ModelState.IsValid)
            {
                if (await this.mountService.DoesExist(model.Name))
                {
                    ModelState.AddModelError("Name", "Invalid Name");
                }

                return this.View(model);
            }

            await this.mountService.Add(model);

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
