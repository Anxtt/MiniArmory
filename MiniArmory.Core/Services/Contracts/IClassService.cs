using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IClassService
    {
        Task Add(ClassFormModel model);

        Task<IEnumerable<ClassViewModel>> AllClasses();
    }
}
