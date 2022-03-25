﻿using MiniArmory.Core.Models;
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

        public void Add(FactionFormModel model)
        {
            Faction faction = new Faction()
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image
            };

            this.db.Factions.Add(faction);
            this.db.SaveChanges();
        }
    }
}