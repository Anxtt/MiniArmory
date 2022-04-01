using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Class;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class ClassService : IClassService
    {
        private readonly MiniArmoryDbContext db;

        public ClassService(MiniArmoryDbContext db)
            => this.db = db;

        public async Task Add(ClassFormModel model)
        {
            Class classEntity = new Class()
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
                Specialisation = model.Specialisation,
                SpecialisationDescription = model.SpecialisationDescription,
                SpecialisationImage = model.SpecialisationImage
            };

            await this.db.Classes.AddAsync(classEntity);
            await this.db.SaveChangesAsync();
        }

        public async Task AddSpells(ClassViewModel model)
        {
            Class classEntity = await this.db
                .Classes
                .Include(x => x.Spells)
                .Where(x => x.Id == model.Id)
                .FirstAsync();

            foreach (var spells in model.Spells)
            {
                Spell spell = await this.db
                    .Spells
                    .Include(x => x.Class)
                    .Where(x => x.Id == spells)
                    .FirstAsync();

                classEntity.Spells.Add(spell);
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClassViewModel>> AllClasses()
            => await this.db
            .Classes
            .Select(x => new ClassViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Specialisation = x.Specialisation,
                Image = x.Image,
                SpecialisationDescription = x.SpecialisationDescription,
                SpecialisationImage = x.SpecialisationImage
            })
            .ToListAsync();

        public Task<ClassViewModel> Details(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClassViewModel> GetClass(int id)
            => await this.db
            .Classes
            .Where(x => x.Id == id)
            .Select(x => new ClassViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Image = x.Image,
                Name = x.Name,
                Specialisation = x.Specialisation,
                SpecialisationDescription = x.SpecialisationDescription,
                SpecialisationImage = x.SpecialisationImage
            })
            .FirstAsync();

        public async Task<IEnumerable<JsonFormModel>> GetSpells()
            => await this.db
            .Spells
            .Where(x => x.Type == "Class")
            .Select(x => new JsonFormModel()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}
