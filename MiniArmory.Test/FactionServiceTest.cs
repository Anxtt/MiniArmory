using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MiniArmory.Core.Models.Faction;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

using NUnit.Framework;

namespace MiniArmory.Test
{
    public class FactionServiceTest
    {
        private IServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IFactionService factionService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IFactionService, FactionService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            factionService = serviceProvider.GetService<IFactionService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddFaction()
        {
            FactionFormModel faction = new FactionFormModel()
            {
                Name = "zxcvb",
                Image = "aaaaaa",
                Description = "zxcvb"
            };

            await factionService.Add(faction);

            Assert.That(await db.Factions
                .AnyAsync(x => x.Name == faction.Name) == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "qwertyu";

            Assert.That(await factionService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "aaaac";

            Assert.That(await factionService.DoesExist(name) == false);
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
                Name = "qwertyu",
                Image = "qwertyu",
                Description = "qwertyu"
            };

            await db.Factions.AddAsync(one);
            await db.SaveChangesAsync();
        }
    }
}
