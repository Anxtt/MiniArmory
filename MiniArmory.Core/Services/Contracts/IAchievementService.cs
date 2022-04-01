using MiniArmory.Core.Models.Achievement;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IAchievementService
    {
        Task Add(AchievementFormModel model);

        Task<IEnumerable<AchievementViewModel>> AllAchievements();
    }
}
