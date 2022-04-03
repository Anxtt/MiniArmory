using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;
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
                ClassImage = x.Class.ClassImage,
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
                Image = model.Image,
                UserId = id
            };

            await this.db.Characters.AddAsync(character);
            await this.db.SaveChangesAsync();
        }

        public async Task AddMountToCharacter(Guid id, string mountName)
        {
            Character character = await this.db
                .Characters
                .Include(x => x.Mounts)
                .Where(x => x.Id == id)
                .FirstAsync();

            Mount mount = await this.db
                .Mounts
                .Include(x => x.Characters)
                .Where(x => x.Name == mountName)
                .FirstAsync();

            character.Mounts.Add(mount);
            character.Mounts
                .Where(x => x.Name == mountName)
                .First()
                .IsCollected = true;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesExist(Guid id)
            => await this.db
            .Characters
            .AnyAsync(x => x.Id == id);

        public async Task EarnRating(Guid id)
        {
            Character character = await this.db
                .Characters
                .Where(x => x.Id == id)
                .FirstAsync();

            var (rating, win, loss) = CalculateRating(character.Rating);

            character.Rating = rating;
            character.Win += win;
            character.Loss += loss;

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
                ClassImage = x.Class.ClassImage,
                FactionName = x.Faction.Name,
                FactionImage = x.Faction.Image,
                RealmName = x.Realm.Name,
                Image = x.Image,
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
                ClassImage = x.Class.ClassImage,
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
                     ClassImage = x.Class.ClassImage,
                     Faction = x.Faction.Image,
                     Name = x.Name,
                     Loss = x.Loss,
                     Realm = x.Realm.Name,
                     Rating = x.Rating,
                     Win = x.Win
                 })
                 .ToListAsync();

        public async Task<bool> RollForReward(string type)
        {
            Random rnd = new Random();

            int chance = rnd.Next(0, 101);

            if ((type == "Mount" && chance < 70) ||
                (type == "Achievement" && chance < 50))
            {
                return false;
            }

            return true;
        }

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
                ClassImage = x.Class.ClassImage
            })
            .ToListAsync();

        public async Task<IEnumerable<MountViewModel>> UnownedMounts(Guid id)
            => await this.db
            .Mounts
            .Where(x => !x.Characters.Any(z => z.Id == id))
            .Select(x => new MountViewModel()
            {
                Name = x.Name,
                Image = x.Image
            })
            .ToListAsync();

        private (short rating, short win, short loss) CalculateRating(short rating)
        {
            Random rnd = new Random();

            short winLose = (short)rnd.Next(-21, 21);
            short win = 0;
            short loss = 0;

            if (winLose >= 0)
            {
                win++;
            }
            else
            {
                loss++;
            }

            if (rating >= 1800)
            {
                rating += winLose;
            }
            else
            {
                win = 1;
                loss = 0;
                rating += 50;
            }

            return (rating, win, loss);
        }
    }
}