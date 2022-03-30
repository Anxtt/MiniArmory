using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IRealmService
    {
        Task Add(RealmFormModel model);

        Task<IEnumerable<RealmViewModel>> AllRealms();
    }
}
