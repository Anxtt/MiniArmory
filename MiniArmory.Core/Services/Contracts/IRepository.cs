namespace MiniArmory.Core.Services.Contracts
{
    public interface IRepository<T> where T : class
    {
        void Add(T model);
    }
}
