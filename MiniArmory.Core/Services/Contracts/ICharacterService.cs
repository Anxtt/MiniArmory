using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ICharacterService
    {
        Task Add(CharacterFormModel model, Guid id);

        Task AddAchievementToCharacter(Guid id, string achievement);

        Task AddMountToCharacter(Guid id, string mountName);

        Task<IEnumerable<LeaderboardViewModel>> AchievementStats();

        Task<bool> DoesExist(Guid id);

        Task EarnRating(Guid id);

        Task<CharacterViewModel> FindCharacterById(Guid id);

        //Task<bool> IsLooking(Guid id);
        Task<Tuple<bool, Guid?>> IsLooking(Guid id);

        Task<IEnumerable<LeaderboardViewModel>> LeaderboardStats();

        Task<LFGFormModel> LFGCharacter(Guid id);

        Task<IEnumerable<CharacterViewModel>> SearchCharacters(string chars);

        bool RollForReward(string type);

        Task<IEnumerable<AchievementViewModel>> OwnAchievements(Guid id);

        Task<IEnumerable<LeaderboardViewModel>> OwnCharacters(Guid id);

        Task<IEnumerable<AchievementViewModel>> UnownedAchievements(Guid id);

        Task<IEnumerable<MountViewModel>> UnownedMounts(Guid id);

        Task SignUp(LFGFormModel model);

        Task<IEnumerable<JsonFormModel>> GetRealms();

        Task TeamUp(Guid id, Guid partnerId);

        Task EarnRatingAsTeam(Guid id, Guid partnerId);

        Task LeaveTeam(Guid id, Guid partnerId);
    }
}
