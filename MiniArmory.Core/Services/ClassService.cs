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
}
}
