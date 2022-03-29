using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class RealmService : IRealmService
    {
        private readonly MiniArmoryDbContext db;

        public RealmService(MiniArmoryDbContext db) 
            => this.db = db;

        public async Task Add(RealmFormModel model)
        {
            Realm realm = new Realm()
            {
                Language = model.Language,
                Name = model.Name
            };
        
            await this.db.Realms.AddAsync(realm);
            await this.db.SaveChangesAsync();
        }
    }
}
