using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data;
using MiniArmory.Data.Models;

using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly MiniArmoryDbContext db;

        public ImageService(MiniArmoryDbContext db)
            => this.db = db;

        public async Task<Guid> Add(ImageFormModel model, Guid characterId)
        {
            Image image = Image.Load(model.OriginalContent);

            image.Metadata.ExifProfile = null;

            ImageData imageData = new ImageData()
            {
                Name = model.FileName,
                OriginalContent = model.OriginalContent,
                ContentType = model.ContentType,
                CharacterId = characterId,
            };

            await this.db.AddAsync(imageData);

            return imageData.Id;
        }

        public async Task<byte[]> ConvertToByteArray(Stream content)
        {
            using (var ms = new MemoryStream())
            {
                await content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public async Task DeleteImage(Guid id)
        {
            ImageData image = await this.db
                .Images
                .FirstOrDefaultAsync(x => x.Id == id);

            this.db.Remove(image!);
        }

        public async Task<string> GetImageById(string type, Guid id)
        {
            ImageQueryModel image = await this.db
                    .Images
                    .Include(x => x.Character)
                    .Where(ExpressionPredicate(type, id))
                    .Select(x => new ImageQueryModel()
                    {
                        ContentType = x.ContentType,
                        OriginalContent = x.OriginalContent
                    })
                    .FirstOrDefaultAsync();

            return ConvertImageToB64(image);
        }

        public string ConvertImageToB64(ImageQueryModel image)
        {
            string mimeType = image.ContentType;
            string base64 = Convert.ToBase64String(image.OriginalContent);

            return string.Format("data:{0};base64,{1}", mimeType, base64);
        }

        private Expression<Func<ImageData, bool>> ExpressionPredicate(string type, Guid id)
            => type.ToLower() switch
            {
                If.CHARACTER => x => x.CharacterId == id,
                _ => x => x.Id == id,
            };
    }
}
