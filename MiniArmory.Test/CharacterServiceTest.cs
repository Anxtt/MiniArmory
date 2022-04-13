using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Models.Character;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;
using NUnit.Framework;

namespace MiniArmory.Test
{
    public class CharacterServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private ICharacterService characterService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<ICharacterService, CharacterService>()
                .AddSingleton<UserManager<User>>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            characterService = serviceProvider.GetService<ICharacterService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddCharacter()
        {
            CharacterFormModel character = new CharacterFormModel()
            {
                Name = "zxcvb",
                Image = "aaaaa",
                Realm = 1,
                Class = 1,
                Faction = 1,
                Race = 1
            };

            User user = await db.Users.FirstAsync();

            await characterService.Add(character, user.Id);

            Assert.That(await db.Characters
                .AnyAsync(x => x.Name == "zxcvb") == true);
        }

        [Test]
        public async Task DoesExistString()
        {
            string name = "qwertyu";

            Assert.That(await characterService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistGuid()
        {
            Character character = await db.Characters.FirstAsync();
            Guid id = character.Id;

            Assert.That(await characterService.DoesExist(id) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalseString()
        {
            string name = "123456";

            Assert.That(await characterService.DoesExist(name) == false);
        }

        [Test]
        public async Task DoesExistReturnsFalseGuid()
        {
            Guid guid = Guid.NewGuid();

            Assert.That(await characterService.DoesExist(guid) == false);
        }

        [Test]
        public async Task AchievementStats()
        {
            IEnumerable<CharacterViewModel> models = await characterService.AchievementStats();

            Assert.That(await db.Characters.CountAsync() == models.Count());
        }

        [Test]
        public async Task AddAchievementToCharacter()
        {
            string achieName = "Gladiator";
            Character character = await db.Characters.FirstAsync();
            Guid id = character.Id;

            await characterService.AddAchievementToCharacter(id, achieName);

            Assert.That(character.Achievements.Any(x => x.Name == achieName) == true);
        }

        [Test]
        public async Task AddMountToCharacter()
        {
            string mountName = "Horse";
            Character character = await db.Characters.FirstAsync();
            Guid id = character.Id;

            await characterService.AddMountToCharacter(id, mountName);

            Assert.That(character.Mounts.Any(x => x.Name == mountName) == true);
        }

        [Test]
        public async Task EarnRating()
        {
            Character character = await db.Characters.FirstAsync();
            Guid id = character.Id;
            int originalRating = character.Rating;

            await characterService.EarnRating(id);

            Assert.That(character.Rating != originalRating);
        }

        [Test]
        public async Task FindCharacterById()
        {
            Character character = await db.Characters.FirstAsync();
            Guid id = character.Id;

            Assert.That(await characterService.FindCharacterById(id) != null);
        }

        [Test]
        public async Task LeaderboardStats()
        {
            IEnumerable<CharacterViewModel> models = await characterService.LeaderboardStats();

            Assert.That(await characterService.LeaderboardStats() != null);
            Assert.That(models.Count() == await db.Characters.CountAsync());
        }

        [Test]
        public async Task OwnAchievements()
        {
            Character character = await db.Characters.FirstAsync();

            IEnumerable<AchievementViewModel> models = await characterService.OwnAchievements(character.Id);

            Assert.That(character.Achievements.Count == models.Count());
        }

        [Test]
        public async Task OwnCharacters()
        {
            User user = await db.Users.FirstAsync();

            IEnumerable<CharacterViewModel> models = await characterService.OwnCharacters(user.Id);

            Assert.That(user.Characters.Count() == models.Count());
        }

        [Test]
        public async Task SearchCharacters()
        {
            string chars = "a";

            IEnumerable<CharacterViewModel> models = await characterService.SearchCharacters(chars);

            Assert.That(models.Count() != 0);
        }

        [Test]
        public async Task UnownedAchievements()
        {
            Character character = await db.Characters.FirstAsync();

            IEnumerable<AchievementViewModel> models = await characterService.UnownedAchievements(character.Id);

            Assert.That(models.Count() != character.Achievements.Count());
        }

        [Test]
        public async Task UnownedMounts()
        {
            Character character = await db.Characters.FirstAsync();

            IEnumerable<MountViewModel> models = await characterService.UnownedMounts(character.Id);

            Assert.That(models.Count() != character.Mounts.Count());
        }

        [Test]
        public async Task SignUp()
        {
            Character character = await db.Characters.FirstAsync();
            LFGFormModel model = new LFGFormModel()
            {
                Id = character.Id,
            };
            
            await characterService.SignUp(model);

            Assert.That(character.IsLooking == true);
        }

        [Test]
        public async Task LFGCharacter()
        {
            Character character = await db.Characters.FirstAsync();
            
            LFGFormModel model = await characterService.LFGCharacter(character.Id);

            Assert.That(model.Name == character.Name);
        }

        [Test]
        public async Task TeamUp()
        {
            Character character = await db.Characters.FirstAsync();
            Character partner = await db.Characters.FirstAsync(x => x.Name != character.Name);

            await characterService.TeamUp(character.Id, partner.Id);

            Assert.That(!character.IsLooking);
            Assert.That(!partner.IsLooking);

            Assert.That(character.PartnerId == partner.Id);
            Assert.That(partner.PartnerId == character.Id);
        }

        [Test]
        public async Task EarnRatingAsTeam()
        {
            Character character = await db.Characters.FirstAsync();
            Character partner = await db.Characters.FirstAsync(x => x.Name != character.Name);

            int characterOriginalRating = character.Rating;
            int partnerOriginalRating = character.Rating;

            await characterService.EarnRatingAsTeam(character.Id, partner.Id);

            Assert.That(character.Rating != characterOriginalRating);
            Assert.That(partner.Rating != partnerOriginalRating);
        }

        [Test]
        public async Task LeaveTeam()
        {
            Character character = await db.Characters.FirstAsync();
            Character partner = await db.Characters.FirstAsync(x => x.Name != character.Name);

            await characterService.LeaveTeam(character.Id, partner.Id);

            Assert.That(character.PartnerId == null);
            Assert.That(partner.PartnerId == null);
        }

        [Test]
        public async Task Delete()
        {
            Character character = await db.Characters.FirstAsync();
            Guid id = character.Id;

            await characterService.Delete(id);

            Assert.That(db.Characters.Any(x => x.Id == id) == false);
        }

        [Test]
        public async Task OwnMounts()
        {
            Character character = await db.Characters.FirstAsync();
            Mount mount = await db.Mounts.FirstAsync();

            character.Mounts.Add(mount);
            await db.SaveChangesAsync();

            IEnumerable<MountViewModel> models = await characterService.OwnMounts(character.Id);

            Assert.That(models.Count() == 1);
        }

        [Test]
        public async Task GetCharacterForChange()
        {
            Character character = await db.Characters.FirstAsync();

            CharacterFormModel model = await characterService.GetCharacterForChange(character.Id);

            Assert.That(model != null);
        }

        [Test]
        public async Task ChangeName()
        {
            Character character = await db.Characters.FirstAsync();

            await characterService.ChangeName(character.Id, "Gosho");

            Assert.That(character.Name == "Gosho");
        }

        [Test]
        public async Task ChangeRace()
        {
            Character character = await db.Characters.FirstAsync();

            Assert.That(character.RaceId == 1);

            await characterService.ChangeRace(character.Id, "2");

            Assert.That(character.RaceId == 2);
        }

        [Test]
        public async Task ChangeFaction()
        {
            Character character = await db.Characters.FirstAsync();

            Assert.That(character.FactionId == 1);

            await characterService.ChangeFaction(character.Id, "2");

            Assert.That(character.FactionId == 2);
        }

        [Test]
        public async Task IsLookingNegativeNoPartner()
        {
            Character character = await db.Characters.FirstAsync();

            await characterService.IsLooking(character.Id);

            Assert.That(character.Partner == null);
            Assert.That(character.IsLooking == false);
        }

        [Test]
        public async Task IsLookingTrueNoPartner()
        {
            Character character = await db.Characters.FirstAsync();
            character.IsLooking = true;

            await db.SaveChangesAsync();

            await characterService.IsLooking(character.Id);

            Assert.That(character.Partner == null);
            Assert.That(character.IsLooking == true);
        }

        [Test]
        public async Task IsLookingFalsePartnerExists()
        {
            Character character = await db.Characters.FirstAsync();
            character.Partner = await db.Characters.FirstAsync(x => x.Name != character.Name);

            await db.SaveChangesAsync();

            await characterService.IsLooking(character.Id);

            Assert.That(character.Partner != null);
            Assert.That(character.IsLooking == false);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Faction faction = new Faction()
            {
                Name = "Horde",
                Image = "aaaaaa",
                Description = "zxcvb"
            };

            Faction factionTwo = new Faction()
            {
                Name = "Alliance",
                Image = "aaaaaa",
                Description = "zxcvb"
            };

            Race race = new Race()
            {
                Arms = "aaaaaa",
                Description = "aaaaaaa",
                Image = "aaaaaa",
                Name = "Orc",
                FactionId = 1,
                SpellId = 1
            };

            Race raceTwo = new Race()
            {
                Arms = "aaaaaa",
                Description = "aaaaaaa",
                Image = "aaaaaa",
                Name = "Human",
                FactionId = 2,
                SpellId = 1
            };

            Class classEntity = new Class()
            {
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "Monk",
                SpecialisationName = "aaaaaa",
                SpecialisationImage = "aaaaaa",
                ClassImage = "aaaaaa",
                SpecialisationDescription = "aaaaa"
            };

            Spell classSpell = new Spell()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "Smite",
                Type = "Class",
                Tooltip = "aaaaaaa",
                Range = 15,
                Cooldown = 15,
                ClassId = 1
            };

            Spell racialSpell = new Spell()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "Blood Fury",
                Type = "Race",
                Tooltip = "aaaaaaa",
                Range = 15,
                Cooldown = 15,
                RaceId = 1
            };

            Spell racialSpellTwo = new Spell()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "Every Man for Himself",
                Type = "Race",
                Tooltip = "aaaaaaa",
                Range = 15,
                Cooldown = 15,
                RaceId = 2
            };

            Realm realm = new Realm()
            {
                Name = "Outland",
                Language = "aaaaa"
            };

            Mount mount = new Mount()
            {
                Name = "Horse",
                Image = "asdfgh",
                GroundSpeed = 80,
                FlyingSpeed = 100
            };

            User userOne = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "123456"
            };

            User userTwo = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "7891011"
            };

            Character one = new Character()
            {
                Name = "qwertyu",
                Image = "aaaaa",
                RealmId = 1,
                ClassId = 1,
                FactionId = 1,
                RaceId = 1,
                UserId = userOne.Id,
                Win = 0,
                Loss = 0
            };

            Character two = new Character()
            {
                Name = "asdfgh",
                Image = "aaaaa",
                RealmId = 1,
                ClassId = 1,
                FactionId = 1,
                RaceId = 1,
                UserId = userTwo.Id,
                Win = 0,
                Loss = 0
            };

            Achievement achie = new Achievement()
            {
                Category = "aaaaa",
                Description = "aaaaa",
                Image = "aaaaa",
                Name = "Gladiator",
                Points = 5
            };

            await db.Factions.AddAsync(faction);
            await db.Factions.AddAsync(factionTwo);
            await db.Realms.AddAsync(realm);
            await db.Races.AddAsync(race);
            await db.Races.AddAsync(raceTwo);
            await db.Classes.AddAsync(classEntity);
            await db.Spells.AddAsync(classSpell);
            await db.Spells.AddAsync(racialSpell);
            await db.Spells.AddAsync(racialSpellTwo);
            await db.Mounts.AddAsync(mount);
            await db.Users.AddAsync(userOne);
            await db.Users.AddAsync(userTwo);
            await db.Characters.AddAsync(one);
            await db.Characters.AddAsync(two);
            await db.Achievements.AddAsync(achie);
            await db.SaveChangesAsync();
        }
    }
}
