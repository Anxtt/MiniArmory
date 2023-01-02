using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Achievement;
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

        public async Task<IEnumerable<CharacterViewModel>> AchievementStats()
            => await this.db
            .Characters
            .Include(x => x.Achievements)
            .Include(x => x.Class)
            .Include(x => x.Faction)
            .Include(x => x.Realm)
            .Select(x => new CharacterViewModel()
            {
                ClassImage = x.Class.ClassImage,
                FactionImage = x.Faction.Image,
                Id = x.Id,
                Name = x.Name,
                RealmName = x.Realm.Name,
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

        public async Task AddAchievementToCharacter(Guid id, string achievement)
        {
            Character character = await this.db
                .Characters
                .Include(x => x.Achievements)
                .Where(x => x.Id == id)
                .FirstAsync();

            Achievement achie = await this.db
                .Achievements
                .Include(x => x.Characters)
                .Where(x => x.Name == achievement)
                .FirstAsync();

            character.Achievements.Add(achie);

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

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesExist(Guid id)
            => await this.db
            .Characters
            .AnyAsync(x => x.Id == id);

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Characters
            .AnyAsync(x => x.Name == name);

        public async Task EarnRating(Guid id)
        {
            Character character = await this.db
                .Characters
                .Where(x => x.Id == id)
                .FirstAsync();

            var (rating, win, loss) = CalculateRating(character.Rating);

            character.Rating += rating;
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

        public async Task<IEnumerable<CharacterViewModel>> LeaderboardStats()
            => await this.db
            .Characters
            .Include(x => x.Faction)
            .Include(x => x.Class)
            .Include(x => x.Realm)
            .Select(x => new CharacterViewModel()
            {
                Id = x.Id,
                ClassImage = x.Class.ClassImage,
                FactionImage = x.Faction.Image,
                Name = x.Name,
                Loss = x.Loss,
                RealmName = x.Realm.Name,
                Rating = x.Rating,
                Win = x.Win,
                Image = x.Image,
                ClassName = x.Class.Name,
                FactionName = x.Faction.Name
            })
            .ToListAsync();

        public async Task<IEnumerable<AchievementViewModel>> OwnAchievements(Guid id)
            => await this.db
                .Achievements
                .Include(x => x.Characters)
                .Where(x => x.Characters.Any(z => z.Id == id))
                .Select(x => new AchievementViewModel()
                {
                    Name = x.Name,
                    Image = x.Image,
                    Category = x.Category,
                    Description = x.Description,
                    Points = x.Points
                })
                .ToListAsync();

        public async Task<IEnumerable<CharacterViewModel>> OwnCharacters(Guid id)
             => await this.db
                 .Characters
                 .Where(x => x.UserId == id)
                 .Include(x => x.Faction)
                 .Include(x => x.Class)
                 .Include(x => x.Realm)
                 .Select(x => new CharacterViewModel()
                 {
                     Id = x.Id,
                     ClassImage = x.Class.ClassImage,
                     FactionImage = x.Faction.Image,
                     Name = x.Name,
                     Loss = x.Loss,
                     RealmName = x.Realm.Name,
                     Rating = x.Rating,
                     Win = x.Win
                 })
                 .ToListAsync();

        public bool RollForReward(string type)
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

        public async Task<IEnumerable<AchievementViewModel>> UnownedAchievements(Guid id)
            => await this.db
                .Achievements
                .Include(x => x.Characters)
                .Where(x => !x.Characters.Any(z => z.Id == id))
                .Select(x => new AchievementViewModel()
                {
                    Name = x.Name,
                    Image = x.Image,
                    Category = x.Category,
                    Description = x.Description,
                    Points = x.Points
                })
                .ToListAsync();

        public async Task<IEnumerable<MountViewModel>> UnownedMounts(Guid id)
            => await this.db
            .Mounts
            .Include(x => x.Characters)
            .Where(x => !x.Characters.Any(z => z.Id == id))
            .Select(x => new MountViewModel()
            {
                Name = x.Name,
                Image = x.Image,
                FlyingSpeed = x.FlyingSpeed,
                GroundSpeed = x.GroundSpeed
            })
            .ToListAsync();

        public async Task SignUp(LFGFormModel model)
        {
            Character character = await this.db
                .Characters
                .Where(x => x.Id == model.Id)
                .FirstAsync();

            character.IsLooking = true;

            await this.db.SaveChangesAsync();
        }

        public async Task<LFGFormModel> LFGCharacter(Guid id)
            => await this.db
                    .Characters
                    .Include(x => x.Faction)
                    .Include(x => x.Race)
                    .Include(x => x.Class)
                    .Include(x => x.Realm)
                    .Include(x => x.User)
                    .Where(x => x.Id == id)
                    .Select(x => new LFGFormModel()
                    {
                        Id = x.Id,
                        ClassImage = x.Class.ClassImage,
                        ClassName = x.Class.Name,
                        FactionImage = x.Faction.Image,
                        Image = x.Image,
                        FactionName = x.Faction.Name,
                        Name = x.Name,
                        Loss = x.Loss,
                        Rating = x.Rating,
                        RealmName = x.Realm.Name,
                        Win = x.Win,
                        CharactersInLFG = this.db
                                .Characters
                                .Include(z => z.User)
                                .Where(z => z.IsLooking == true && z.PartnerId == null && z.User.Id != x.User.Id)
                                .Select(z => new CharacterViewModel()
                                {
                                    Id = z.Id,
                                    ClassImage = z.Class.ClassImage,
                                    FactionImage = z.Faction.Image,
                                    Name = z.Name,
                                    Loss = z.Loss,
                                    Rating = z.Rating,
                                    RealmName = z.Realm.Name,
                                    Win = z.Win,
                                })
                                .ToList()
                    })
                    .FirstAsync();

        public async Task TeamUp(Guid id, Guid partnerId)
        {
            Character character = await this.db
                .Characters
                .Where(x => x.Id == id)
                .FirstAsync();

            Character partner = await this.db
                .Characters
                .Where(x => x.Id == partnerId)
                .FirstAsync();

            character.Partner = partner;
            partner.Partner = character;

            character.IsLooking = false;
            partner.IsLooking = false;

            await this.db.SaveChangesAsync();
        }

        public async Task<Tuple<bool, Guid?>> IsLooking(Guid id)
            => await this.db
               .Characters
               .Where(x => x.Id == id)
               .Select(x => new Tuple<bool, Guid?>(x.IsLooking, x.PartnerId))
               .FirstAsync();

        public async Task EarnRatingAsTeam(Guid id, Guid partnerId)
        {
            Character character = await this.db
                .Characters
                .Where(x => x.Id == id)
                .FirstAsync();

            Character partner = await this.db
                .Characters
                .Where(x => x.Id == partnerId)
                .FirstAsync();

            var (rating, win, loss) = CalculateRating(character.Rating);

            if (character.Rating < 1800 && partner.Rating < 1800)
            {
                character.Rating += rating;
                character.Win += win;
                character.Loss += loss;

                partner.Rating += rating;
                partner.Win += win;
                partner.Loss += loss;
            }
            else if (character.Rating < 1800)
            {
                character.Rating += rating;
                character.Win += win;
                character.Loss += loss;

                partner.Win += win;
            }
            else if (partner.Rating < 1800)
            {
                (rating, win, loss) = CalculateRating(partner.Rating);

                partner.Rating += rating;
                partner.Win += win;
                partner.Loss += loss;

                character.Win += win;
            }
            else
            {
                character.Rating += rating;
                character.Win += win;
                character.Loss += loss;

                partner.Rating += rating;
                partner.Win += win;
                partner.Loss += loss;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task LeaveTeam(Guid id, Guid partnerId)
        {
            Character character = await this.db
                .Characters
                .Include(x => x.Partner)
                .Where(x => x.Id == id)
                .FirstAsync();

            Character partner = await this.db
                .Characters
                .Include(x => x.Partner)
                .Where(x => x.Id == partnerId)
                .FirstAsync();

            character.Partner = null;
            partner.Partner = null;

            await this.db.SaveChangesAsync();
        }

        private (int rating, short win, short loss) CalculateRating(int rating)
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

            if (rating < 1800)
            {
                win = 1;
                loss = 0;
                winLose = 50;
            }

            return (winLose, win, loss);
        }

        public async Task Delete(Guid id)
        {
            Character character = await this.db
                .Characters
                .Include(x => x.Partner)
                .Where(x => x.Id == id)
                .FirstAsync();

            if (character.Partner != null)
            {
                await this.LeaveTeam(character.Id, character.Partner.Id);
            }

            this.db.Characters.Remove(character);
            await this.db.SaveChangesAsync();
        }

        public async Task ChangeFaction(Guid id, string factionId)
        {
            Character character = await this.db
                .Characters
                .Include(x => x.Faction)
                .Where(x => x.Id == id)
                .FirstAsync();

            character.FactionId = int.Parse(factionId);

            await this.db.SaveChangesAsync();
        }

        public async Task ChangeName(Guid id, string name)
        {
            Character character = await this.db
                .Characters
                .Where(x => x.Id == id)
                .FirstAsync();

            character.Name = name;

            await this.db.SaveChangesAsync();
        }

        public async Task ChangeRace(Guid id, string raceId)
        {
            Character character = await this.db
                .Characters
                .Include(x => x.Race)
                .Where(x => x.Id == id)
                .FirstAsync();

            character.RaceId = int.Parse(raceId);

            await this.db.SaveChangesAsync();
        }

        public async Task<CharacterFormModel> GetCharacterForChange(Guid id)
            => await this.db
            .Characters
            .Where(x => x.Id == id)
            .Select(x => new CharacterFormModel()
            {
                Name = x.Name,
                Faction = x.FactionId,
                Race = x.RaceId
            })
            .FirstAsync();

        public async Task<IEnumerable<MountViewModel>> OwnMounts(Guid id)
            => await this.db
            .Mounts
            .Include(x => x.Characters)
            .Where(x => x.Characters.Any(z => z.Id == id))
            .Select(x => new MountViewModel()
            {
                FlyingSpeed = x.FlyingSpeed,
                GroundSpeed = x.GroundSpeed,
                Image = x.Image,
                Name = x.Name
            })
            .ToListAsync();
    }
}