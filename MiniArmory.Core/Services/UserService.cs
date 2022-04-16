using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class UserService : IUserService
    {
        private readonly MiniArmoryDbContext db;

        public UserService(MiniArmoryDbContext db) 
            => this.db = db;

        public async Task<int> AchievementsCount()
            => await this.db
            .Achievements
            .CountAsync();

        public async Task<int> ArenasCount()
            => await this.db
            .Characters
            .SumAsync(x => x.Win + x.Loss);

        public async Task<IEnumerable<UserViewModel>> AllUsers()
            => await this.db
            .Users
            .Select(x => new UserViewModel()
            {
                Id = x.Id,
                Name = x.UserName
            })
            .ToListAsync();

        public async Task<IEnumerable<GuidJsonFormModel>> GetRoles()
            => await this.db
            .Roles
            .Select(x => new GuidJsonFormModel()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        public async Task<User> GetUserById(Guid id)
            => await this.db
            .Users
            .FirstAsync(x => x.Id == id);

        public async Task<int> HighestRating()
            => await this.db
            .Characters
            .MaxAsync(x => x.Rating);

        public async Task<JsonFormModel> MostPlayedClass()
            => await this.db
            .Characters
            .Include(x => x.Class)
            .GroupBy(x => x.Class.Name)
            .Select(x => new JsonFormModel()
            {
                Name = x.Key,
                Id = x.Count()
            })
            .OrderByDescending(x => x.Id)
            .ThenBy(x => x.Name)
            .FirstAsync();

        public async Task<JsonFormModel> MostPlayedFaction()
            => await this.db
            .Characters
            .GroupBy(x => x.Faction.Name)
            .Select(x => new JsonFormModel()
            {
                Name = x.Key,
                Id = x.Count()
            })
            .OrderByDescending(x => x.Id)
            .FirstAsync();

        public async Task<JsonFormModel> MostPlayedRace()
            => await this.db
            .Characters
            .GroupBy(x => x.Race.Name)
            .Select(x => new JsonFormModel()
            {
                Name = x.Key,
                Id = x.Count()
            })
            .OrderByDescending(x => x.Id)
            .ThenBy(x => x.Name)
            .FirstAsync();

        public async Task<JsonFormModel> MostPopulatedServer()
            => await this.db
            .Characters
            .GroupBy(x => x.Realm.Name)
            .Select(x => new JsonFormModel()
            {
                Name = x.Key,
                Id = x.Count()
            })
            .OrderByDescending(x => x.Id)
            .ThenBy(x => x.Name)
            .FirstAsync();

        public async Task<int> UsersCount()
            => await this.db
            .Characters
            .CountAsync();

        public async Task<UserRolesViewModel> FindUserById(Guid id)
            => await this.db
            .Users
            .Where(x => x.Id == id)
            .Select(x => new UserRolesViewModel()
            {
                Id = x.Id,
                Name = x.UserName
            })
            .FirstAsync();
    }
}
