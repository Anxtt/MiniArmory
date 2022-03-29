using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IClassService : IRepository<ClassFormModel>
    {
        Task<IEnumerable<ClassViewModel>> AllClasses();
    }
}
