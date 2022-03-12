using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class SpellService : ISpellService
    {
        private readonly MiniArmoryDbContext db;

        public SpellService(MiniArmoryDbContext db) 
            => this.db = db;

        public void Add(SpellFormModel model)
        {
            Spell spell = new Spell()
            {
                Cooldown = model.Cooldown,
                Description = model.Description,
                IsRacial = model.IsRacial,
                Name = model.Name,
                Range = model.Range,
                Tooltip = model.Tooltip,
                Class = null,
                Race = null,
                ClassId = 0,
                RaceId = 0
            };

            this.db.Spells.Add(spell);
            this.db.SaveChanges();
        }
    }
}
