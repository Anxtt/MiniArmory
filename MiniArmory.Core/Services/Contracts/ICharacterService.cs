using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ICharacterService
    {
        Task Add(CharacterFormModel model, Guid id);

        Task<IEnumerable<LeaderboardViewModel>> AchievementStats();

        Task<CharacterViewModel> FindCharacterById(Guid id);

        Task<IEnumerable<LeaderboardViewModel>> LeaderboardStats();

        Task<IEnumerable<CharacterViewModel>> SearchCharacters(string chars);

        Task<IEnumerable<LeaderboardViewModel>> OwnCharacters(Guid id);

        Task<IEnumerable<JsonFormModel>> GetRealms();
    }
}
