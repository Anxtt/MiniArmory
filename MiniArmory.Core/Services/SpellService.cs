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
                Tooltip = model.Tooltip
            };

            Class classEntity = this.db
                .Classes
                .First(x => x.Id == int.Parse(model.Class));

            if (!model.IsRacial && model.Class != null)
            {
                spell.ClassId = classEntity.Id;
            }

            if (model.IsRacial)
            {
                //spell.RaceId = model.RacialId;
            }

            this.db.Spells.Add(spell);
            this.db.SaveChanges();
        }

        public IEnumerable<ClassSpellFormModel> GetClasses()
        {
            var classes = this.db
                .Classes
                .Select(x => new ClassSpellFormModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return classes;
        }

        public IEnumerable<RaceSpellFormModel> GetRaces()
        {
            var classes = this.db
                .Races
                .Select(x => new RaceSpellFormModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return classes;
        }
    }
}
