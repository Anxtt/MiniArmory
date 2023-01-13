using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

using NUnit.Framework;

namespace MiniArmory.Test
{
    public class MountServiceRealm
    {
        private IServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IMountService mountService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IMountService, MountService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            mountService = serviceProvider.GetService<IMountService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddRealm()
        {
            MountFormModel mount = new MountFormModel()
            {
                Name = "zxcvb",
                FlyingSpeed = 100,
                GroundSpeed = 100,
                Image = "aaaaaa"
            };

            await mountService.Add(mount);

            Assert.That(await db.Mounts
                .AnyAsync(x => x.Name == mount.Name) == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "qwertyu";

            Assert.That(await mountService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "aaaac";

            Assert.That(await mountService.DoesExist(name) == false);
        }

        [Test]
        public async Task AllRealms()
        {
            IEnumerable<MountViewModel> mounts = await mountService.AllMounts();

            Assert.That(mounts.Count() == db.Mounts.Count());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Mount one = new Mount()
            {
                Name = "qwertyu",
                Image = "qwertyu",
                GroundSpeed = 80,
                FlyingSpeed = 100
            };

            Mount two = new Mount()
            {
                Name = "asdfgh",
                Image = "asdfgh",
                GroundSpeed = 80,
                FlyingSpeed = 100
            };

            await db.Mounts.AddAsync(one);
            await db.Mounts.AddAsync(two);
            await db.SaveChangesAsync();
        }
    }
}
