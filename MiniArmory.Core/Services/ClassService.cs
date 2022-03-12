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

        public void Add(ClassFormModel model)
        {
            Class classEntity = new Class()
            {
                Description = model.Description,
                Name = model.Name,
                SpecialisationDescription = model.SpecialisationDescription,
                SpecialisationImage = model.SpecialisationImage
            };

            this.db.Classes.Add(classEntity);
            this.db.SaveChanges();
        }
}
}
