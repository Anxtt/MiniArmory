using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class RaceService : IRaceService
    {
        private readonly MiniArmoryDbContext db;

        public RaceService(MiniArmoryDbContext db) 
            => this.db = db;

        public async Task Add(RaceFormModel model)
        {
            Faction faction = await this.db
                .Factions
                .FirstAsync(x => x.Id == int.Parse(model.Faction));

            Spell spell = await this.db
                .Spells
                .FirstAsync(x => x.Id == int.Parse(model.RacialSpell));

            Race race = new Race()
            {
                Description = model.Description,
                Faction = faction,
                FactionId = faction.Id, 
                Name = model.Name,
                RacialSpell = spell,
                SpellId = spell.Id,
            };

            await this.db.Races.AddAsync(race);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<RaceViewModel>> AllRaces()
            => await this.db
            .Races
            .Include(x => x.RacialSpell)
            .Select(x => new RaceViewModel()
            {
                Description = x.Description,
                Faction = x.Faction.Name,
                FactionImage = x.Faction.Image,
                Name = x.Name,
                Arms = x.Arms,
                Id = x.Id,
                RacialSpell = x.RacialSpell.Name,
                RacialSpellImage = x.RacialSpell.Tooltip,
                RacialSpellDescription = x.RacialSpell.Description,
            })
            .ToListAsync();

        public async Task<IEnumerable<JsonFormModel>> GetRacialSpells()
            => await this.db
            .Spells
            .Where(x => x.Race == null && x.Type == "Race")
            .Select(x => new JsonFormModel()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}
