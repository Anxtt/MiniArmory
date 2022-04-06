using MiniArmory.Core.Models.Realm;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IRealmService
    {
        Task Add(RealmFormModel model);

        Task<IEnumerable<RealmViewModel>> AllRealms();

        Task<bool> DoesExist(string name);
    }
}
