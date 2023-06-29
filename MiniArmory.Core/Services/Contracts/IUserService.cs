using MiniArmory.Core.Models;

using MiniArmory.Data.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<int> UsersCount();

        Task<int> AchievementsCount();

        Task<int> ArenasCount();

        Task<int> HighestRating();

        Task<IEnumerable<UserViewModel>> AllUsers();

        Task<UserRolesViewModel> FindUserById(Guid id);

        Task<JsonFormModel> MostPlayedFaction();

        Task<JsonFormModel> MostPlayedRace();

        Task<JsonFormModel> MostPlayedClass();

        Task<JsonFormModel> MostPopulatedServer();

        Task<User> GetUserById(Guid id);

        Task<IEnumerable<GuidJsonFormModel>> GetRoles();
    }
}
