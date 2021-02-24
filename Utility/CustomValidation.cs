using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Utility
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private int maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return null;
            }
            if (file.Length > 2 * 1024 * 1024)
            {
                return new ValidationResult($"Max file size allowed is {maxFileSize} MB", new List<string> { file.Name });
            }
            return ValidationResult.Success;
        }
    }
    public class OnlyImagesAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] extensions = new string[] { ".jpg", ".jpeg", ".png" };
            var file = value as IFormFile;
            if (file == null)
            {
                return null;
            }
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if(extensions.Any(x=>x == fileExtension))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Not a acceptable image file",new List<string> { "CoverImage"});
        }
    }
}
