using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MiniArmory.Core.Models.Race;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

using NUnit.Framework;

namespace MiniArmory.Test
{
    public class RaceServiceTest
    {
        private IServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IRaceService raceService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IRaceService, RaceService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            raceService = serviceProvider.GetService<IRaceService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddRace()
        {
            RaceFormModel race = new RaceFormModel()
            {
                Description = "aaaaaaaaa",
                Name = "zxcvb",
                Faction = "1",
                Image = "aaaaa",
                Arms = "aaaaaa",
                RacialSpell = "1"
            };

            await raceService.Add(race);

            Assert.That(await db.Races
                .AnyAsync(x => x.Name == "zxcvb") == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "Orc";

            Assert.That(await raceService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "Undead";

            Assert.That(await raceService.DoesExist(name) == false);
        }

        [Test]
        public async Task AllRaces()
        {
            IEnumerable<RaceViewModel> races = await raceService.AllRaces();

            Assert.That(races.Count() == db.Races.Count());
        }

        [Test]
        public async Task GetRace()
        {
            RaceViewModel races = await raceService.GetRace(1);

            Assert.IsNotNull(races);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Faction one = new Faction()
            {
                Name = "Horde",
                Image = "aaaaaa",
                Description = "zxcvb"
            };

            Race two = new Race()
            {
                Arms = "aaaaaa",
                Description = "aaaaaaa",
                Image = "aaaaaa",
                Name = "Orc",
                FactionId = 1
            };

            Race three = new Race()
            {
                Arms = "aaaaaa",
                Description = "aaaaaaa",
                Image = "aaaaaa",
                Name = "Blood Elf",
                FactionId = 1
            };

            Spell spell = new Spell()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "Will of the Forsaken",
                Type = "Race",
                Tooltip = "aaaaaaa",
                Range = 15,
                Cooldown = 15
            };

            await db.Factions.AddAsync(one);
            await db.Races.AddAsync(two);
            await db.Races.AddAsync(three);
            await db.Spells.AddAsync(spell);
            await db.SaveChangesAsync();
        }
    }
}
