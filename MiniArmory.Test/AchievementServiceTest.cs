using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniArmory.Core.Models.Achievement;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;
using NUnit.Framework;

namespace MiniArmory.Test
{
    public class AchievementServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IAchievementService achieService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IAchievementService, AchievementService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            achieService = serviceProvider.GetService<IAchievementService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddAchievement()
        {
            AchievementFormModel achie = new AchievementFormModel()
            {
                Category = "asd",
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "zxcvb",
                Points = 5
            };

            await achieService.Add(achie);

            Assert.That(await db.Achievements
                .AnyAsync(x => x.Name == achie.Name) == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "qwertyu";

            Assert.That(await achieService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "aaaac";

            Assert.That(await achieService.DoesExist(name) == false);
        }

        [Test]
        public async Task AllAchievements()
        {
            IEnumerable<AchievementViewModel> achies = await achieService.AllAchievements();

            Assert.That(achies.Count() == db.Achievements.Count());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Achievement one = new Achievement()
            {
                Category = "asd",
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "qwertyu",
                Points = 5,
            };

            Achievement two = new Achievement()
            {
                Category = "asd",
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "asdfgh",
                Points = 5,
            };

            await db.Achievements.AddAsync(one);
            await db.Achievements.AddAsync(two);
            await db.SaveChangesAsync();
        }
    }
}
