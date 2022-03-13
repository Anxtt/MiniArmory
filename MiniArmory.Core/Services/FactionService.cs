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
    public class FactionService : IFactionService
    {
        private readonly MiniArmoryDbContext db;

        public FactionService(MiniArmoryDbContext db) 
            => this.db = db;

        public void AddFaction(FactionFormModel model)
        {
            Faction faction = new Faction()
            {
                Description = model.Description,
                Image = model.Image
            };

            this.db.Factions.Add(faction);
            this.db.SaveChanges();
        }
    }
}
