using Microsoft.AspNetCore.Mvc;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

namespace MiniArmory.Web.Controllers
{
    public class MountController : Controller
    {
        private readonly IMountService mountSevice;

        public MountController(IMountService mountSevice) 
            => this.mountSevice = mountSevice;

        public IActionResult AddMount() 
            => this.View(); 
        
        [HttpPost]
        public async Task<IActionResult> AddMount(MountFormModel model)
        {
            await this.mountSevice.Add(model);

            return this.View();
        }
    }
}
