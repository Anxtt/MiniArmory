using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Spell;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ISpellService
    {
        Task Add(SpellFormModel model);

        Task<IEnumerable<SpellViewModel>> AllSpells();

        Task<SpellListViewModel> AllSpells(int pageNo, int pageSize);

        Task DeleteSpell(string name);

        Task<bool> DoesExist(string name);

        Task EditSpell(SpellFormModel model);

        Task<IEnumerable<SpellViewModel>> FilteredSpells(string type);

        Task<SpellListViewModel> FilteredSpells(string type, int pageNo, int pageSize);

        Task<SpellFormModel> FindSpell(string name);

        Task<IEnumerable<JsonFormModel>> GetClasses(); 

        Task<IEnumerable<JsonFormModel>> GetRaces();

        Task<IEnumerable<JsonFormModel>> GetSameFactionRaces(int? raceId, int? factionId);

        Task<IEnumerable<JsonFormModel>> GetOppositeFactionRaces(int? factionId);
    }
}
