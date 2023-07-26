using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Core.Services;
using MiniArmory.Data.Models;
using MiniArmory.Data;
using NUnit.Framework;
using System.IO;
using MiniArmory.Core.Models;
using Microsoft.EntityFrameworkCore;
using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Test
{
    public class ImageServiceTest
    {
        private IServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        private IImageService imageService;
        private MiniArmoryDbContext db;

        [SetUp]
        public async Task SetUp()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(x => dbContext.CreateContext())
                .AddSingleton<IImageService, ImageService>()
                .BuildServiceProvider();

            db = serviceProvider.GetService<MiniArmoryDbContext>();
            imageService = serviceProvider.GetService<IImageService>();

            await SeedDbAsync();
        }

        [Test]
        public async Task AddCharacterImage()
        {
            Stream stream = File.OpenRead("../../../Images/favicon.jpeg");

            byte[] bytes = await this.imageService.ConvertToByteArray(stream);

            ImageFormModel model = new ImageFormModel()
            {
                ContentType = "image/jpeg",
                FileName = "image.jpeg",
                OriginalContent = bytes
            };

            Character character = await this.db
                .Characters
                .FirstOrDefaultAsync(x => x.Name == "one");

            Guid imageId = await this.imageService.AddCharacterImage(model, character.Id);

            Assert.That(imageId != default);
        }

        [Test]
        public async Task ConvertToByteArray()
        {
            Stream stream = File.OpenRead("../../../Images/favicon.jpeg");

            byte[] bytes = await this.imageService.ConvertToByteArray(stream);

            Assert.That(bytes.Length > 0);
        }

        [Test]
        public async Task ConvertImageToB64()
        {
            ImageQueryModel model = new ImageQueryModel()
            {
                ContentType = "image/jpeg",
                OriginalContent = await this.db
                    .Images
                    .Select(x => x.OriginalContent)
                    .FirstAsync()
            };

            string b64 = this.imageService.ConvertImageToB64(model);

            Assert.That(b64.GetType() == typeof(string));
        }

        [Test]
        public async Task DeleteImage()
        {
            Guid id = Guid.Parse("9407791a-598d-47d0-9cdd-4169ed7b824f");

            int originalCount = await this.db
                .Images
                .CountAsync();

            await this.imageService.DeleteImage(id);

            Assert.That(originalCount != await this.db.Images.CountAsync());
        }

        [Test]
        public async Task GetImageByCharacterId()
        {
            Guid id = Guid.Parse("3d391569-ea2f-44a7-89a1-8db519a602d5");

            string b64 = await this.imageService.GetImageById(If.CHARACTER, id);

            Assert.That(b64 != default);
        }

        [Test]
        public async Task GetImageByImageId()
        {
            Guid id = Guid.Parse("9407791a-598d-47d0-9cdd-4169ed7b824f");

            string b64 = await this.imageService.GetImageById("", id);

            Assert.That(b64 != default);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.db.DisposeAsync();
        }

        private async Task SeedDbAsync()
        {
            Faction faction = new Faction()
            {
                Name = "Horde",
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

            Realm realm = new Realm()
            {
                Name = "Outland",
                Language = "aaaaa"
            };

            User userOne = new User()
            {
                UserName = "123456",
                Id = Guid.NewGuid()
            };

            ImageData image = new ImageData()
            {
                Id = Guid.Parse("9407791a-598d-47d0-9cdd-4169ed7b824f"),
                ContentType = "image/jpeg",
                Name = "image",
                OriginalContent = new byte[] { 1, 2, 3, 4, 5, 6}
            };

            Character one = new Character()
            {
                Id = Guid.Parse("3d391569-ea2f-44a7-89a1-8db519a602d5"),
                Name = "one",
                RealmId = 1,
                ClassId = 1,
                FactionId = 1,
                RaceId = 1,
                UserId = userOne.Id,
                Win = 0,
                Loss = 0,
                Image = image,
                ImageId = image.Id
            };

            await this.db.Factions.AddAsync(faction);
                  
            await this.db.Realms.AddAsync(realm);
                  
            await this.db.Races.AddAsync(race);
                  
            await this.db.Classes.AddAsync(classEntity);
                  
            await this.db.Spells.AddAsync(classSpell);
                  
            await this.db.Spells.AddAsync(racialSpell);

            await this.db.Users.AddAsync(userOne);

            await this.db.Images.AddAsync(image);

            await this.db.Characters.AddAsync(one);

            await this.db.SaveChangesAsync();
        }
    }
}
