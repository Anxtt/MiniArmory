using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models.Faction;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class FactionService : IFactionService
    {
        private readonly MiniArmoryDbContext db;

        public FactionService(MiniArmoryDbContext db) 
            => this.db = db;

        public async Task Add(FactionFormModel model)
        {
            Faction faction = new Faction()
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image
            };

            await this.db.Factions.AddAsync(faction);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Factions
            .AnyAsync(x => x.Name == name);
    }
}
