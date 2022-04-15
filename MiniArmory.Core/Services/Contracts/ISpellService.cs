using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Spell;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ISpellService
    {
        Task Add(SpellFormModel model);

        Task<bool> DoesExist(string name);

        Task<IEnumerable<JsonFormModel>> GetClasses(); 

        Task<IEnumerable<JsonFormModel>> GetRaces();

        Task<IEnumerable<JsonFormModel>> GetSameFactionRaces(int? raceId, int? factionId);

        Task<IEnumerable<JsonFormModel>> GetOppositeFactionRaces(int? factionId);

        Task<IEnumerable<SpellViewModel>> AllSpells();

        Task<IEnumerable<SpellViewModel>> FilteredSpells(string type);
    }
}
