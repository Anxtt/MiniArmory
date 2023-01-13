using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MiniArmory.Core.Models.Spell;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

using NUnit.Framework;

namespace MiniArmory.Test
{
    public class SpellServiceTest
    {
        private IServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private ISpellService spellService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<ISpellService, SpellService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            spellService = serviceProvider.GetService<ISpellService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddClassSpell()
        {
            SpellFormModel spell = new SpellFormModel()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "zxcvb",
                Type = "Class",
                Tooltip = "aaaaaaa",
                Range = 5,
                Cooldown = 5,
                Class = "1"
            };

            await spellService.Add(spell);

            Assert.That(await db.Spells
                .AnyAsync(x => x.ClassId == 1) == true);
        }

        [Test]
        public async Task AddRacialSpell()
        {
            SpellFormModel spell = new SpellFormModel()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "zxcvb",
                Type = "Race",
                Tooltip = "aaaaaaa",
                Range = 15,
                Cooldown = 15,
                Race = "1"
            };

            await spellService.Add(spell);

            Assert.That(await db.Spells
                .AnyAsync(x => x.RaceId == 1) == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "qwertyu";

            Assert.That(await spellService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "aaaac";

            Assert.That(await spellService.DoesExist(name) == false);
        }

        [Test]
        public async Task AllSpells()
        {
            IEnumerable<SpellViewModel> spells = await spellService.AllSpells();

            Assert.That(spells.Count() == db.Spells.Count());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Spell one = new Spell()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "qwertyu",
                Cooldown = 5,
                Range = 5,
                Tooltip = "aaaaa",
                Type = "aaaaa",
            };

            Spell two = new Spell()
            {
                Description = "aaaaaaaaaaaaaa",
                Name = "asdfgh",
                Cooldown = 10,
                Range = 10,
                Tooltip = "bbbbb",
                Type = "bbbbb"
            };

            Class three = new Class()
            {
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "zxcvb",
                ClassImage = "zxcvb",
                SpecialisationDescription = "aaaaaaaaaaa",
                SpecialisationImage = "aaaaaaaaa",
                SpecialisationName = "zxcvb"
            };

            Faction four = new Faction()
            {
                Name = "zxcvb",
                Image = "aaaaaa",
                Description = "zxcvb"
            };

            Race five = new Race()
            {
                Arms = "aaaaaa",
                Description = "aaaaaaa",
                Image = "aaaaaa",
                Name = "Orc",
                FactionId = 1
            };

            await db.Spells.AddAsync(one);
            await db.Spells.AddAsync(two);
            await db.Classes.AddAsync(three);
            await db.Factions.AddAsync(four);
            await db.Races.AddAsync(five);
            await db.SaveChangesAsync();
        }
    }
}
