using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ICharacterService
    {
        Task<Guid> Add(CharacterFormModel model, Guid id);

        Task AddAchievementToCharacter(Guid id, string achievement);

        Task AddMountToCharacter(Guid id, string mountName);

        Task<IEnumerable<CharacterViewModel>> AchievementStats();

        Task ChangeFaction(Guid id, string factionId);

        Task ChangeImage(Guid id, ImageFormModel model);

        Task ChangeName(Guid id, string name);

        Task ChangeRace(Guid id, string raceId);

        Task Delete(Guid id);

        Task<bool> DoesExist(Guid id);

        Task<bool> DoesExist(string name);

        Task EarnRating(Guid id);

        Task EarnRatingAsTeam(Guid id, Guid partnerId);

        Task<Tuple<string, string, string>> EarnRatingAsTeamVsTeam(Guid id, Guid partnerId);

        Task<CharacterViewModel> FindCharacterById(Guid id);

        Task<CharacterFormModel> GetCharacterForChange(Guid id);
        
        Task<IEnumerable<JsonFormModel>> GetRealms();

        Task<Tuple<bool, Guid?>> IsLooking(Guid id);

        Task<IEnumerable<CharacterViewModel>> LeaderboardStats();

        Task LeaveTeam(Guid id, Guid partnerId);

        Task<LFGFormModel> LFGCharacter(Guid id);

        Task<IEnumerable<AchievementViewModel>> OwnAchievements(Guid id);

        Task<IEnumerable<CharacterViewModel>> OwnCharacters(Guid id);

        Task<IEnumerable<MountViewModel>> OwnMounts(Guid id);

        bool RollForReward(string type);

        Task<IEnumerable<CharacterViewModel>> SearchCharacters(string chars);

        Task SignUp(LFGFormModel model);

        Task TeamUp(Guid id, Guid partnerId);

        Task<IEnumerable<CharacterViewModel>> Top3();

        Task<IEnumerable<AchievementViewModel>> UnownedAchievements(Guid id);

        Task<IEnumerable<MountViewModel>> UnownedMounts(Guid id);
    }
}
