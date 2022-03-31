using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ICharacterService
    {
        Task Add(CharacterFormModel model, Guid id);

        Task<IEnumerable<LeaderboardViewModel>> LeaderboardStats();

        Task<IEnumerable<JsonFormModel>> GetRealms();
    }
}
