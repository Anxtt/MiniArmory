using MiniArmory.Core.Models;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface ISpellService : IRepository<SpellFormModel>
    {
        IEnumerable<ClassSpellFormModel> GetClasses(); 

        IEnumerable<RaceSpellFormModel> GetRaces(); 
    }
}
