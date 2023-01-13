using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Class;
using MiniArmory.Core.Models.Spell;
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
                ClassImage = model.ClassImage,
                SpecialisationName = model.SpecialisationName,
                SpecialisationDescription = model.SpecialisationDescription,
                SpecialisationImage = model.SpecialisationImage
            };

            await this.db.Classes.AddAsync(classEntity);
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
                SpecialisationName = x.SpecialisationName,
                Image = x.Image,
                ClassImage = x.ClassImage,
                SpecialisationDescription = x.SpecialisationDescription,
                SpecialisationImage = x.SpecialisationImage
            })
            .ToListAsync();

        public async Task<ClassViewModel> Details(int id)
            => await this.db
                .Classes
                .Where(x => x.Id == id)
                .Include(x => x.Spells)
                .Select(x => new ClassViewModel()
                {
                    ClassImage = x.ClassImage,
                    Description = x.Description,
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.Name,
                    SpecialisationDescription = x.SpecialisationDescription,
                    SpecialisationName = x.SpecialisationName,
                    SpecialisationImage = x.SpecialisationImage,
                    Spells = x.Spells.Select(z => new SpellViewModel()
                    {
                        Description = z.Description,
                        Name = z.Name,
                        Tooltip = z.Tooltip
                    })
                    .ToList()
                })
                .FirstAsync();

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Classes
            .AnyAsync(x => x.Name == name);

        public async Task<bool> DoesExist(int id)
            => await this.db
            .Classes
            .AnyAsync(x => x.Id == id);

        public async Task<ClassViewModel> GetClass(int id)
            => await this.db
            .Classes
            .Where(x => x.Id == id)
            .Select(x => new ClassViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                ClassImage = x.ClassImage,
                Image = x.Image,
                Name = x.Name,
                SpecialisationName = x.SpecialisationName,
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
