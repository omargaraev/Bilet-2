using Bilet_2.Models;
using Microsoft.AspNetCore.Http;

namespace Bilet_2.Utilities.Constants
{
    public static class FileExtensions
    {
        public static bool CheckImageType(this IFormFile file,string contentType )
        {

            return file.ContentType.Contains(contentType);
        }
        public static bool CheckImageSize(this IFormFile file, double size)
        {

            return file.Length / 1024 < 200;
        }
    }
}

