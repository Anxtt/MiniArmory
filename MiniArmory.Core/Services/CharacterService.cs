using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly MiniArmoryDbContext db;

        public CharacterService(MiniArmoryDbContext db)
            => this.db = db;

        public async Task<IEnumerable<LeaderboardViewModel>> AchievementStats()
            => await this.db
            .Characters
            .Include(x => x.Achievements)
            .Include(x => x.Class)
            .Include(x => x.Faction)
            .Include(x => x.Realm)
            .Select(x => new LeaderboardViewModel()
            {
                Class = x.Class.Image,
                Faction = x.Faction.Image,
                Id = x.Id,
                Name = x.Name,
                Realm = x.Realm.Name,
                Rating = x.Achievements.Sum(z => z.Points)
            })
            .ToListAsync();

        public async Task Add(CharacterFormModel model, Guid id)
        {
            Character character = new Character()
            {
                Name = model.Name,
                RealmId = model.Realm,
                FactionId = model.Faction,
                RaceId = model.Race,
                ClassId = model.Class,
                UserId = id
            };

            await this.db.Characters.AddAsync(character);
            await this.db.SaveChangesAsync();
        }

        public async Task<CharacterViewModel> FindCharacterById(Guid id)
            => await this.db
            .Characters
            .Where(x => x.Id == id)
            .Include(x => x.Achievements)
            .Include(x => x.Class)
            .Include(x => x.Race)
            .Include(x => x.Faction)
            .Include(x => x.Mounts)
            .Include(x => x.Realm)
            .Select(x => new CharacterViewModel()
            {
                Id = id,
                Name = x.Name,
                ClassName = x.Class.Name,
                ClassImage = x.Class.Image,
                FactionName = x.Faction.Name,
                FactionImage = x.Faction.Image,
                RealmName = x.Realm.Name,
                Rating = x.Rating,
                Win = x.Win,
                Loss = x.Loss
            })
            .FirstAsync();

        public async Task<IEnumerable<JsonFormModel>> GetRealms()
            => await this.db
            .Realms
            .Select(x => new JsonFormModel()
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync();

        public async Task<IEnumerable<LeaderboardViewModel>> LeaderboardStats()
            => await this.db
            .Characters
            .Include(x => x.Faction)
            .Include(x => x.Class)
            .Include(x => x.Realm)
            .Select(x => new LeaderboardViewModel()
            {
                Id = x.Id,
                Class = x.Class.Image,
                Faction = x.Faction.Image,
                Name = x.Name,
                Loss = x.Loss,
                Realm = x.Realm.Name,
                Rating = x.Rating,
                Win = x.Win
            })
            .ToListAsync();

        public async Task<IEnumerable<LeaderboardViewModel>> OwnCharacters(Guid id)
             => await this.db
                 .Characters
                 .Where(x => x.UserId == id)
                 .Include(x => x.Faction)
                 .Include(x => x.Class)
                 .Include(x => x.Realm)
                 .Select(x => new LeaderboardViewModel()
                 {
                     Id = x.Id,
                     Class = x.Class.Image,
                     Faction = x.Faction.Image,
                     Name = x.Name,
                     Loss = x.Loss,
                     Realm = x.Realm.Name,
                     Rating = x.Rating,
                     Win = x.Win
                 })
                 .ToListAsync();

        public async Task<IEnumerable<CharacterViewModel>> SearchCharacters(string chars)
            => await this.db
            .Characters
            .Include(x => x.Class)
            .Include(x => x.Faction)
            .Include(x => x.Realm)
            .Where(x => x.Name.ToLower().Contains(chars.ToLower()))
            .Select(x => new CharacterViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Rating = x.Rating,
                RealmName = x.Realm.Name,
                ClassName = x.Class.Name,
                ClassImage = x.Class.Image
            })
            .ToListAsync();
    }
}