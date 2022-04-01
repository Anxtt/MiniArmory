using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Race;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IRaceService
    {
        Task Add(RaceFormModel model);

        Task<IEnumerable<RaceViewModel>> AllRaces();

        Task<IEnumerable<JsonFormModel>> GetRacialSpells();

        Task<RaceViewModel> GetRace(int id);
    }
}
