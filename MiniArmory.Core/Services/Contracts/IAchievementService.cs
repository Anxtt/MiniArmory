using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IAchievementService : IRepository<AchievementFormModel>
    {
        Task<IEnumerable<AchievementViewModel>> AllAchievements();
    }
}
