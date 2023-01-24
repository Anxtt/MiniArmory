namespace MiniArmory.Core.Services.Contracts
{
    public interface IRedisService
    {
        Task SetCache<T>(string key, T value);

        Task<T> RetrieveCache<T>(string key);
    }
}
