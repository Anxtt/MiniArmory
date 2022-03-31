using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ICharacterService
    {
        Task Add(CharacterFormModel model, Guid id);

        Task<CharacterViewModel> FindCharacterById(Guid id);

        Task<IEnumerable<CharacterViewModel>> SearchCharacters(string chars);
        
        Task<IEnumerable<LeaderboardViewModel>> LeaderboardStats();

        Task<IEnumerable<JsonFormModel>> GetRealms();
    }
}
