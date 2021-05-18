using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Delpin.Mvc.Services
{
    public class ImageConverter
    {
        public async Task<byte[]> ConvertImageToByteArray(IFormFile image)
        {
            await using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public string ConvertByteArrayToBase64String(byte[] image)
        {
            return "data:image/jpeg;base64," + Convert.ToBase64String(image);
        }
    }
}
