using Microsoft.EntityFrameworkCore;
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
                    .FirstAsync<Class>(x => x.Id == int.Parse(model.Class));

                spell.ClassId = classEntity.Id;
                spell.Class = classEntity;
            }
            else if (model.Type == "Race")
            {
                Race race = await this.db
                    .Races
                    .Include(x => x.Faction)
                    .Include(x => x.RacialSpell)
                    .FirstAsync<Race>(x => x.Id == int.Parse(model.Race));

                spell.RaceId = race.Id;
                spell.Race = race;
            }

            await this.db.Spells.AddAsync(spell);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClassSpellFormModel>> GetClasses()
        {
            var classes = await this.db
                .Classes
                .Select(x => new ClassSpellFormModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return classes;
        }

        public async Task<IEnumerable<RaceSpellFormModel>> GetRaces()
        {
            var classes = await this.db
                .Races
                .Select(x => new RaceSpellFormModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return classes;
        }
    }
}
