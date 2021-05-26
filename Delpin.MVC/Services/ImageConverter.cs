using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Delpin.Mvc.Services
{
    public class ImageConverter
    {
        // Converts IFormFile image to a byte array for storing in database
        public async Task<byte[]> ConvertImageToByteArray(IFormFile image)
        {
            await using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }


        // Converts byte array from database to Base64 string to display through HTML
        public string ConvertByteArrayToBase64String(byte[] image)
        {
            return "data:image/jpeg;base64," + Convert.ToBase64String(image);
        }
    }
}
