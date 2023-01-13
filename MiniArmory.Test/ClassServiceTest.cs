using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MiniArmory.Core.Models.Class;
using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

using NUnit.Framework;

namespace MiniArmory.Test
{
    public class ClassServiceTest
    {
        private IServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IClassService classService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IClassService, ClassService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            classService = serviceProvider.GetService<IClassService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddClass()
        {
            ClassFormModel classEntity = new ClassFormModel()
            {
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "zxcvb",
                ClassImage = "zxcvb",
                SpecialisationDescription = "aaaaaaaaaaa",
                SpecialisationImage = "aaaaaaaaa",
                SpecialisationName = "zxcvb"
            };

            await classService.Add(classEntity);

            Assert.That(await db.Classes
                .AnyAsync(x => x.Name == classEntity.Name) == true);
        }

        [Test]
        public async Task DoesExist()
        {
            string name = "qwertyu";

            Assert.That(await classService.DoesExist(name) == true);
        }

        [Test]
        public async Task DoesExistReturnsFalse()
        {
            string name = "aaaac";

            Assert.That(await classService.DoesExist(name) == false);
        }

        [Test]
        public async Task AllClasses()
        {
            IEnumerable<ClassViewModel> classEntities = await classService.AllClasses();

            Assert.That(classEntities.Count() == db.Classes.Count());
        }

        [Test]
        public async Task Details()
        {
            ClassViewModel classViewModel = await classService.Details(1);

            Assert.IsNotNull(classViewModel);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync()
        {
            Class one = new Class()
            {
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "qwertyu",
                SpecialisationName = "aaaaaa",
                SpecialisationImage = "aaaaaa",
                ClassImage = "aaaaaa",
                SpecialisationDescription = "aaaaa"
            };

            Class two = new Class()
            {
                Description = "aaaaaaaaaaaaaa",
                Image = "aaaaaaaa",
                Name = "asdfgh",
                SpecialisationName = "bbbbb",
                SpecialisationImage = "bbbbb",
                ClassImage = "bbbbb",
                SpecialisationDescription = "bbbbb"
            };

            await db.Classes.AddAsync(one);
            await db.Classes.AddAsync(two);
            await db.SaveChangesAsync();
        }
    }
}
