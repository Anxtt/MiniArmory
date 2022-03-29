using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
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
            if (model.Faction == "-1")
            {
                model.Faction = null;
            }

            Mount mount = new Mount()
            {
                FlyingSpeed = model.FlyingSpeed,
                Faction = model.Faction,
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
                Faction = x.Faction,
                FlyingSpeed = x.FlyingSpeed,
                GroundSpeed = x.GroundSpeed,
                Image = x.Image,
                Name = x.Name
            })
            .ToListAsync();

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
