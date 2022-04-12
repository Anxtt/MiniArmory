using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Class;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IClassService
    {
        Task Add(ClassFormModel model);

        Task<IEnumerable<ClassViewModel>> AllClasses();

        Task<ClassViewModel> Details(int id);

        Task<bool> DoesExist(string name);

        Task<ClassViewModel> GetClass(int id);

        Task<IEnumerable<JsonFormModel>> GetSpells();
    }
}
