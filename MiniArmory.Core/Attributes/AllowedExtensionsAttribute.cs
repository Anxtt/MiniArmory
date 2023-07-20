using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace MiniArmory.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] allowedExtensions;

        public AllowedExtensionsAttribute()
        {
            AllowedExtensions = new string[]
            {
                ".jpg",
                ".jpeg",
                ".png",
            };
        }

        public AllowedExtensionsAttribute(string[] allowedExtensions)
        {
            AllowedExtensions = allowedExtensions;
        }

        public string[] AllowedExtensions
        {
            get { return this.allowedExtensions; }
            init
            {
                if (value == default)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.allowedExtensions = value;
            }
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;

            if (file != null)
            {
                string extension = Path.GetExtension(file!.FileName).ToLower();

                if (allowedExtensions.Contains(extension) == false)
                {
                    return new ValidationResult("Not a supported file.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
