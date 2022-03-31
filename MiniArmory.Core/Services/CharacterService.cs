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
                Class = x.Class.Image,
                Faction = x.Faction.Image,
                Name = x.Name,
                Loss = x.Loss,
                Realm = x.Realm.Name,
                Rating = x.Rating,
                Win = x.Win
            })
            .ToListAsync();

        public async Task<IEnumerable<LeaderboardViewModel>> SearchCharacters(string chars)
            => await this.db
            .Characters
            .Include(x => x.Class)
            .Where(x => x.Name.ToLower().Contains(chars.ToLower()))
            .Select(x => new LeaderboardViewModel()
            {
                Name = x.Name,
                Rating = x.Rating,
                Class = x.Class.Image,
            })
            .ToListAsync();
    }
}
