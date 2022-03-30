using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IMountService
    {
        Task Add(MountFormModel model);

        Task<IEnumerable<MountViewModel>> AllMounts();

        Task<IEnumerable<JsonFormModel>> GetFactions();
    }
}
