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

        public void Add(MountFormModel model)
        {
            Mount mount = new Mount()
            {
                FlyingSpeed = model.FlyingSpeed,
                Faction = model.Faction,
                GroundSpeed = model.GroundSpeed,
                Image = model.Image,
                Name = model.Name
            };

            this.db.Mounts.Add(mount);
            this.db.SaveChanges();
        }
    }
}
