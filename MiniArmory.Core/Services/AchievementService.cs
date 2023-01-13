using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models.Achievement;
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

        public async Task Add(AchievementFormModel model)
        {
            Achievement achi = new Achievement()
            {
                Category = model.Category,
                Name = model.Name,
                Description = model.Description,
                Points = model.Points,
                Image = model.Image
            };

            await this.db.Achievements.AddAsync(achi);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AchievementViewModel>> AllAchievements()
            => await this.db
            .Achievements
            .Select(x => new AchievementViewModel()
            {
                Category = x.Category,
                Description = x.Description,
                Name = x.Name,
                Points = x.Points,
                Image = x.Image
            })
            .ToListAsync();

        public async Task<bool> DoesExist(string name)
            => await this.db
            .Achievements
            .AnyAsync(x => x.Name == name);
    }
}
