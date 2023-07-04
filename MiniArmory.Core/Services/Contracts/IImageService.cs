using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IImageService
    {
        Task<Guid> Add(ImageFormModel model, Guid characterId);

        Task<byte[]> ConvertToByteArray(Stream content);

        string ConvertImageToB64(ImageQueryModel image);

        Task DeleteImage(Guid id);

        Task<string> GetImageById(string type, Guid characterId);
    }
}
