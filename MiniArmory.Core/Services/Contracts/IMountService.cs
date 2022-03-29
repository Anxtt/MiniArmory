using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IMountService : IRepository<MountFormModel>
    {
        Task<IEnumerable<MountViewModel>> AllMounts();

        Task<IEnumerable<JsonFormModel>> GetFactions();
    }
}
