using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniArmory.Core.Models.Realm;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;
using NUnit.Framework;

namespace MiniArmory.Test
{
    public class RealmServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IRealmService realmService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IRealmService, RealmService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            realmService = serviceProvider.GetService<IRealmService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddRealm()
        {
            RealmFormModel realm = new RealmFormModel()
            {
                Language = "zxcvb",
                Name = "wwwww"
            };

            await realmService.Add(realm);

            Assert.That(await db.Realms
                .AnyAsync(x => x.Name == realm.Name) == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "qwertyu";

            Assert.That(await realmService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "aaaac";

            Assert.That(await realmService.DoesExist(name) == false);
        }

        [Test]
        public async Task AllRealms()
        {
            IEnumerable<RealmViewModel> realms = await realmService.AllRealms();

            Assert.That(realms.Count() == db.Realms.Count());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Realm one = new Realm()
            {
                Name = "qwertyu",
                Language = "aaaaa"
            };

            Realm two = new Realm()
            {
                Name = "asdfgh",
                Language = "ccccc"
            };

            await db.Realms.AddAsync(one);
            await db.Realms.AddAsync(two);
            await db.SaveChangesAsync();
        }
    }
}
