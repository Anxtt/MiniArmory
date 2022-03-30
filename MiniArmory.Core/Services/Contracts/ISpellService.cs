using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ISpellService
    {
        Task Add(SpellFormModel model);

        Task<IEnumerable<JsonFormModel>> GetClasses(); 

        Task<IEnumerable<JsonFormModel>> GetRaces();

        Task<IEnumerable<SpellViewModel>> AllSpells();
    }
}
