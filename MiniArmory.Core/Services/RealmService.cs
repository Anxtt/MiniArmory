using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Add(RealmFormModel model)
        {
            Realm realm = new Realm()
            {
                Language = model.Language,
                Name = model.Name,
                Population = model.Population
            };
        
            this.db.Realms.Add(realm);
            this.db.SaveChanges();
        }
    }
}
