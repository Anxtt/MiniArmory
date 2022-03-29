using Microsoft.EntityFrameworkCore;
using MiniArmory.Core.Models;
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

        public async Task<IEnumerable<ClassViewModel>> AllClasses()
            => await this.db
            .Classes
            .Select(x => new ClassViewModel()
            {
                Description = x.Description,
                Name = x.Name,
                Specialisation = x.Specialisation,
                Image = x.Image,
                SpecialisationDescription = x.SpecialisationDescription,
                SpecialisationImage = x.SpecialisationImage
            })
            .ToListAsync();
    }
}
