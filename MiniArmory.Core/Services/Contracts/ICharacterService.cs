using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ICharacterService
    {
        Task Add(CharacterFormModel model, Guid id);

        Task AddMountToCharacter(Guid id, string mountName);

        Task<IEnumerable<LeaderboardViewModel>> AchievementStats();

        Task<bool> DoesExist(Guid id);

        Task EarnRating(Guid id);

        Task<CharacterViewModel> FindCharacterById(Guid id);

        Task<IEnumerable<LeaderboardViewModel>> LeaderboardStats();

        Task<IEnumerable<CharacterViewModel>> SearchCharacters(string chars);

        Task<bool> RollForReward(string type);

        Task<IEnumerable<LeaderboardViewModel>> OwnCharacters(Guid id);

        Task<IEnumerable<MountViewModel>> UnownedMounts(Guid id);

        Task<IEnumerable<JsonFormModel>> GetRealms();
    }
}
