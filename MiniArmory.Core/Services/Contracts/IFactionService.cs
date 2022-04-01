using MiniArmory.Core.Models.Faction;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IFactionService
    {
        Task Add(FactionFormModel model);
    }
}
