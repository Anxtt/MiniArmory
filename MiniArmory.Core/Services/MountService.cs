using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Mount;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class MountService : IMountService
    {
        private readonly MiniArmoryDbContext db;

        public MountService(MiniArmoryDbContext db)
            => this.db = db;

        public async Task Add(MountFormModel model)
        {
            Mount mount = new Mount()
            {
                FlyingSpeed = model.FlyingSpeed,
                GroundSpeed = model.GroundSpeed,
                Image = model.Image,
                Name = model.Name
            };

            await this.db.Mounts.AddAsync(mount);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<MountViewModel>> AllMounts()
            => await this.db
            .Mounts
            .Select(x => new MountViewModel()
            {
                FlyingSpeed = x.FlyingSpeed,
                GroundSpeed = x.GroundSpeed,
                Image = x.Image,
                Name = x.Name
            })
            .ToListAsync();

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Mounts
            .AnyAsync(x => x.Name == name);

        public async Task<IEnumerable<JsonFormModel>> GetFactions()
            => await this.db
            .Factions
            .Select(x => new JsonFormModel()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}
