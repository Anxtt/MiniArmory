using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data;
using MiniArmory.Data.Models;

namespace MiniArmory.Core.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly MiniArmoryDbContext db;
        private readonly IImageService imageService;

        public CharacterService(MiniArmoryDbContext db, IImageService imageService)
            => (this.db, this.imageService) = (db, imageService);

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

        public async Task<Guid> Add(CharacterFormModel model, Guid id)
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

            Guid imageId = await this.imageService.AddCharacterImage(new ImageFormModel()
            {
                ContentType = model.Image.ContentType,
                FileName = model.Image.FileName,
                OriginalContent = await this.imageService.ConvertToByteArray(model.Image.OpenReadStream())
            }, character.Id);

            character.ImageId = imageId;

            await this.db.Characters.AddAsync(character);
            await this.db.SaveChangesAsync();

            return character.Id;
        }

        public async Task AddAchievementToCharacter(Guid id, string achievement)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Achievements)
                .FirstAsync();

            Achievement achie = await this.db
                .Achievements
                .Where(x => x.Name == achievement)
                .Include(x => x.Characters)
                .FirstAsync();

            character.Achievements.Add(achie);

            await this.db.SaveChangesAsync();
        }

        public async Task AddMountToCharacter(Guid id, string mountName)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Mounts)
                .FirstAsync();

            Mount mount = await this.db
                .Mounts
                .Where(x => x.Name == mountName)
                .Include(x => x.Characters)
                .FirstAsync();

            character.Mounts.Add(mount);

            await this.db.SaveChangesAsync();
        }

        public async Task ChangeFaction(Guid id, string factionId)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Faction)
                .FirstAsync();

            character.FactionId = int.Parse(factionId);

            await this.db.SaveChangesAsync();
        }

        public async Task ChangeImage(Guid id, ImageFormModel model)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Image)
                .FirstAsync();

            character.Image.ContentType = model.ContentType;
            character.Image.OriginalContent = model.OriginalContent;

            await this.db.SaveChangesAsync();
        }

        public async Task ChangeName(Guid id, string name)
        {
            Character character = await QueryableCharacterById(id)
                .FirstAsync();

            character.Name = name;

            await this.db.SaveChangesAsync();
        }

        public async Task ChangeRace(Guid id, string raceId)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Race)
                .FirstAsync();

            character.RaceId = int.Parse(raceId);

            await this.db.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Partner)
                .Include(x => x.Image)
                .FirstAsync();

            if (character.Partner != null)
            {
                await this.LeaveTeam(character.Id, character.Partner.Id);
            }

            if (character.Image != null)
            {
                await this.imageService.DeleteImage(character.ImageId);
            }

            this.db.Characters.Remove(character);
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
            Character character = await QueryableCharacterById(id)
                .FirstAsync();

            var (rating, win, loss) = CalculateRating(character.Rating);

            character.Rating += rating;
            character.Win += win;
            character.Loss += loss;

            await this.db.SaveChangesAsync();
        }

        public async Task EarnRatingAsTeam(Guid id, Guid partnerId)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Partner)
                .FirstAsync();

            var (rating, win, loss) = CalculateRating(character.Rating);

            CalculateRatingAsTeam(character, character.Partner!, rating, win, loss);

            await this.db.SaveChangesAsync();
        }

        public async Task<Tuple<string, string, string>> EarnRatingAsTeamVsTeam(Guid id, Guid partnerId)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Partner)
                .FirstAsync();

            Character enemy = await this.FindEnemyTeam(character, character.Partner!);

            string status = this.CalculateRatingAsTeamVsTeam(character, enemy);
            await this.db.SaveChangesAsync();

            return new Tuple<string, string, string>(enemy.Name, enemy.Partner!.Name, status);
        }

        public async Task<CharacterViewModel> FindCharacterById(Guid id)
            => await QueryableCharacterById(id)
            .Include(x => x.Achievements)
            .Include(x => x.Class)
            .Include(x => x.Race)
            .Include(x => x.Faction)
            .Include(x => x.Mounts)
            .Include(x => x.Realm)
            .Include(x => x.Image)
            .Select(x => new CharacterViewModel()
            {
                Id = id,
                Name = x.Name,
                ClassName = x.Class.Name,
                ClassImage = x.Class.ClassImage,
                FactionName = x.Faction.Name,
                FactionImage = x.Faction.Image,
                Image = this.imageService.ConvertImageToB64(new ImageQueryModel()
                {
                    ContentType = x.Image.ContentType,
                    OriginalContent = x.Image.OriginalContent,
                }),
                RealmName = x.Realm.Name,
                Rating = x.Rating,
                Win = x.Win,
                Loss = x.Loss
            })
            .FirstAsync();

        public async Task<CharacterFormModel> GetCharacterForChange(Guid id)
            => await QueryableCharacterById(id)
            .Select(x => new CharacterFormModel()
            {
                Name = x.Name,
                Faction = x.FactionId,
                Race = x.RaceId
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

        public async Task<Tuple<bool, Guid?>> IsLooking(Guid id)
            => await QueryableCharacterById(id)
               .Select(x => new Tuple<bool, Guid?>(x.IsLooking, x.PartnerId))
               .FirstAsync();

        public async Task<IEnumerable<CharacterViewModel>> LeaderboardStats()
            => await this.db
            .Characters
            .Include(x => x.Faction)
            .Include(x => x.Class)
            .Include(x => x.Realm)
            .Include(x => x.Image)
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
                Image = this.imageService.ConvertImageToB64(new ImageQueryModel()
                {
                    OriginalContent = x.Image.OriginalContent,
                    ContentType = x.Image.ContentType
                }),
                ClassName = x.Class.Name,
                FactionName = x.Faction.Name
            })
            .ToListAsync();

        public async Task LeaveTeam(Guid id, Guid partnerId)
        {
            Character character = await QueryableCharacterById(id)
                .Include(x => x.Partner)
                .FirstAsync();

            Character partner = await QueryableCharacterById(id)
                .Include(x => x.Partner)
                .FirstAsync();

            character.Partner = null;
            partner.Partner = null;

            await this.db.SaveChangesAsync();
        }

        public async Task<LFGFormModel> LFGCharacter(Guid id)
            => await QueryableCharacterById(id)
            .Include(x => x.Faction)
            .Include(x => x.Race)
            .Include(x => x.Class)
            .Include(x => x.Realm)
            .Include(x => x.User)
            .Include(x => x.Image)
            .Select(x => new LFGFormModel()
            {
                Id = x.Id,
                ClassImage = x.Class.ClassImage,
                ClassName = x.Class.Name,
                FactionImage = x.Faction.Image,
                Image = this.imageService.ConvertImageToB64(new ImageQueryModel()
                {
                    OriginalContent = x.Image.OriginalContent,
                    ContentType = x.Image.ContentType
                }),
                FactionName = x.Faction.Name,
                Name = x.Name,
                Loss = x.Loss,
                Rating = x.Rating,
                RealmName = x.Realm.Name,
                Win = x.Win,
                CharactersInLFG = this.db
                        .Characters
                        .Where(z => z.IsLooking == true && z.PartnerId == null)
                        .Include(z => z.User)
                        .Where(z => z.User.Id != x.User.Id)
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

        public async Task<IEnumerable<AchievementViewModel>> OwnAchievements(Guid id)
            => await this.db
                .Achievements
                .Where(x => x.Characters.Any(z => z.Id == id))
                .Include(x => x.Characters)
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
                 .Include(x => x.Partner)
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
                     PartnerId = x.PartnerId
                 })
                 .ToListAsync();

        public async Task<IEnumerable<MountViewModel>> OwnMounts(Guid id)
            => await this.db
            .Mounts
            .Where(x => x.Characters.Any(z => z.Id == id))
            .Include(x => x.Characters)
            .Select(x => new MountViewModel()
            {
                FlyingSpeed = x.FlyingSpeed,
                GroundSpeed = x.GroundSpeed,
                Image = x.Image,
                Name = x.Name
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
            .Where(x => x.Name.ToLower().Contains(chars.ToLower()))
            .Include(x => x.Class)
            .Include(x => x.Faction)
            .Include(x => x.Realm)
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

        public async Task SignUp(LFGFormModel model)
        {
            Character character = await QueryableCharacterById(model.Id)
                .FirstAsync();

            character.IsLooking = true;

            await this.db.SaveChangesAsync();
        }

        public async Task TeamUp(Guid id, Guid partnerId)
        {
            Character character = await QueryableCharacterById(id)
                .FirstAsync();

            Character partner = await QueryableCharacterById(partnerId)
                .FirstAsync();

            character.Partner = partner;
            partner.Partner = character;

            character.IsLooking = false;
            partner.IsLooking = false;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CharacterViewModel>> Top3()
        => await this.db
            .Characters
            .Include(x => x.Image)
            .Select(x => new CharacterViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Loss = x.Loss,
                Rating = x.Rating,
                Win = x.Win,
                Image = this.imageService.ConvertImageToB64(new ImageQueryModel()
                {
                    OriginalContent = x.Image.OriginalContent,
                    ContentType = x.Image.ContentType
                })
            })
            .OrderByDescending(x => x.Rating)
            .ThenBy(x => x.Name)
            .Take(3)
            .ToListAsync();

        public async Task<IEnumerable<AchievementViewModel>> UnownedAchievements(Guid id)
            => await this.db
                .Achievements
                .Where(x => !x.Characters.Any(z => z.Id == id))
                .Include(x => x.Characters)
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
            .Where(x => !x.Characters.Any(z => z.Id == id))
            .Include(x => x.Characters)
            .Select(x => new MountViewModel()
            {
                Name = x.Name,
                Image = x.Image,
                FlyingSpeed = x.FlyingSpeed,
                GroundSpeed = x.GroundSpeed
            })
            .ToListAsync();

        private (int rating, short win, short loss) CalculateRating(int rating)
        {
            Random rnd = new Random();

            short winLose = (short)rnd.Next(-21, 21);
            short win = 0;
            short loss = 0;

            if (rating < 1800)
            {
                win = 1;
                loss = 0;
                winLose = 50;

                return (winLose, win, loss);
            }

            if (winLose >= 0)
            {
                win++;
            }
            else
            {
                loss++;
            }

            return (winLose, win, loss);
        }

        private void CalculateRatingAsTeam(Character character, Character partner, int rating, short win, short loss)
        {
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
        }

        private string CalculateRatingAsTeamVsTeam(Character firstCaptain, Character secondCaptain)
        {
            Random rnd = new Random();
            int first = rnd.Next(0, 100) + 1;
            int second = rnd.Next(0, 100) + 1;

            if (first > second)
            {
                firstCaptain.Win += 1;
                firstCaptain.Rating += 16;

                firstCaptain.Partner.Win += 1;
                firstCaptain.Partner.Rating += 16;

                secondCaptain.Loss += 1;
                secondCaptain.Rating -= 12;

                secondCaptain.Partner.Loss += 1;
                secondCaptain.Partner.Rating -= 12;

                return $"You won against {secondCaptain.Name} and {secondCaptain.Partner.Name}";
            }
            else
            {
                secondCaptain.Win += 1;
                secondCaptain.Rating += 16;

                secondCaptain.Partner.Win += 1;
                secondCaptain.Partner.Rating += 16;

                firstCaptain.Loss += 1;
                firstCaptain.Rating -= 12;

                firstCaptain.Partner.Loss += 1;
                firstCaptain.Partner.Rating -= 12;

                return $"You lost against {secondCaptain.Name} and {secondCaptain.Partner.Name}";
            }
        }

        private async Task<Character> FindEnemyTeam(Character character, Character partner)
        {
            IList<Character> chars = await this.db
                    .Characters
                    .Include(x => x.User)
                    .Include(x => x.Partner)
                    .Where(x => (x.Id != character.Id && x.Id != partner.Id) && x.Partner != null)
                    //.Select(x => new Character() {}) breaks the query for some reason
                    //Second Captain changes aren't saved in the db
                    //e.g. win/loss doesn't change, neither does rating
                    .ToListAsync();

            Random rnd = new Random();
            return chars[rnd.Next(chars.Count())];
        }

        private IQueryable<Character> QueryableCharacterById(Guid id)
            => this.db
            .Characters
            .Where(x => x.Id == id);
    }
}