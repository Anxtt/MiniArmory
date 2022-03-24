using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;
using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Core.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly MiniArmoryDbContext db;

        public AchievementService(MiniArmoryDbContext db) 
            => this.db = db;

        public void Add(AchievementFormModel model)
        {
            Achievement achi = new Achievement()
            {
                Category = model.Category,
                Name = model.Name,
                Description = model.Description,
                Points = model.Points
            };

            this.db.Achievements.Add(achi);
            this.db.SaveChanges();
        }
    }
}
