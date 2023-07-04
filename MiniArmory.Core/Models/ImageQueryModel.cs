namespace MiniArmory.Core.Models
{
    public class ImageQueryModel
    {
        public string B64Content { get; set; }

        public string ContentType { get; set; }

        public byte[] OriginalContent { get; set; }
    }
}
