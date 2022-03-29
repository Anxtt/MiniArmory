using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IRaceService : IRepository<RaceFormModel>
    {
        Task<IEnumerable<RaceViewModel>> AllRaces();

        Task<IEnumerable<JsonFormModel>> GetRacialSpells();
    }
}
