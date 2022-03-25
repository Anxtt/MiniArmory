using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ISpellService : IRepository<SpellFormModel>
    {
        Task<IEnumerable<ClassSpellFormModel>> GetClasses(); 

        Task<IEnumerable<RaceSpellFormModel>> GetRaces();
    }
}
