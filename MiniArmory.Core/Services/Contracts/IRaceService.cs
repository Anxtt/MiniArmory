using MiniArmory.Core.Models;

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
