using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Models;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data;
using MiniArmory.Data.Models;

using static MiniArmory.GlobalConstants.Core;
using static MiniArmory.GlobalConstants.Web;

namespace MiniArmory.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly MiniArmoryDbContext db;

        public ImageService(MiniArmoryDbContext db)
            => this.db = db;

        public async Task<Guid> AddCharacterImage(ImageFormModel model, Guid characterId)
        {
            ImageData imageData = new ImageData()
            {
                Name = model.FileName,
                ContentType = model.ContentType,
                CharacterId = characterId
            };

            return await AddImage(model.OriginalContent, imageData, If.CHARACTER);
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

        private async Task<Guid> AddImage(byte[] content, ImageData imageData, string type)
        {
            using (Image image = ModifyImage(content, type))
            {
                using (var ms = new MemoryStream())
                {
                    image.SaveAsJpeg(ms);

                    imageData.OriginalContent = ms.ToArray();

                    await this.db.AddAsync(imageData);

                    return imageData.Id;
                }
            }
        }

        private Image ModifyImage(byte[] content, string type)
        {
            Image image = Image.Load(content);

            image.Metadata.ExifProfile = null;

            image.Mutate(x => x
            .AutoOrient()
            .Resize(SizerOptions(type)));

            return image;
        }

        private Expression<Func<ImageData, bool>> ExpressionPredicate(string type, Guid id)
            => type.ToLower() switch
            {
                If.CHARACTER => x => x.CharacterId == id,
                _ => x => x.Id == id,
            };

        private ResizeOptions SizerOptions(string type)
        => type.ToLower() switch
        {
            If.CHARACTER => new ResizeOptions()
            {
                Size = new Size(ImageConst.CharacterWidth, ImageConst.CharacterHeight),
                Position = AnchorPositionMode.Center,
                Mode = ResizeMode.Crop
            },
            _ => new ResizeOptions()
        };
    }
}
