﻿using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models.Realm;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data;
using MiniArmory.Data.Models;

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

        public async Task<IEnumerable<RealmViewModel>> AllRealms()
            => await this.db
            .Realms
            .Select(x => new RealmViewModel()
            {
                Name = x.Name,
                Language = x.Language,
                Population = x.Characters.Count > 5 ? "High" : 
                             x.Characters.Count >= 3 && x.Characters.Count <= 5 ? "Medium" :
                             "Low"
            })
            .ToListAsync();

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Realms
            .AnyAsync(x => x.Name == name);
    }
}
