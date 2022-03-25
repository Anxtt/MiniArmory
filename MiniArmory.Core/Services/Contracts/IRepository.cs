namespace MiniArmory.Core.Services.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task Add(T model);
    }
}
