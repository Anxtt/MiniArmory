using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Spell;
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

        public async Task Add(SpellFormModel model)
        {
            Spell spell = new Spell()
            {
                Cooldown = model.Cooldown,
                Description = model.Description,
                Name = model.Name,
                Range = model.Range,
                Type = model.Type,
                Tooltip = model.Tooltip
            };

            if (model.Type == "Class")
            {
                Class classEntity = await this.db
                    .Classes
                    .FirstAsync(x => x.Id == int.Parse(model.Class));

                spell.ClassId = classEntity.Id;
                spell.Class = classEntity;
            }
            else if (model.Type == "Race" && model.Race != "-1")
            {
                Race race = await this.db
                    .Races
                    .Include(x => x.Faction)
                    .Include(x => x.RacialSpell)
                    .FirstAsync(x => x.Id == int.Parse(model.Race));

                spell.RaceId = race.Id;
                spell.Race = race;
            }

            await this.db.Spells.AddAsync(spell);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpellViewModel>> AllSpells()
            => await this.db
            .Spells
            .Select(x => new SpellViewModel()
            {
                Name = x.Name,
                Description = x.Description,
                Tooltip = x.Tooltip
            })
            .ToListAsync();

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Spells
            .AnyAsync(x => x.Name == name);

        public async Task<IEnumerable<JsonFormModel>> GetClasses()
            => await this.db
                .Classes
                .Select(x => new JsonFormModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

        public async Task<IEnumerable<JsonFormModel>> GetRaces()
            => await this.db
                .Races
                .Select(x => new JsonFormModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

        public async Task<IEnumerable<JsonFormModel>> GetSameFactionRaces(int? raceId, int? factionId)
            => await this.db
                   .Races
                   .Where(x => x.Id != raceId && x.FactionId == factionId)
                   .Select(x => new JsonFormModel()
                   {
                       Id = x.Id,
                       Name = x.Name
                   })
                   .ToListAsync();

        public async Task<IEnumerable<JsonFormModel>> GetOppositeFactionRaces(int? factionId)
            => await this.db
                   .Races
                   .Where(x => x.FactionId != factionId)
                   .Select(x => new JsonFormModel()
                   {
                       Id = x.Id,
                       Name = x.Name
                   })
                   .ToListAsync();
    }
}
