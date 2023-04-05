using Microsoft.AspNetCore.Components.Forms;

namespace ECommerce.Client.Classes
{
    public class ImageUploadHandler
    {
        //max allowed size is ~1MB (1,000,000 Bytes)
        private static readonly int MaxSize = 1024 * 1024;

        private static readonly string[]
            AcceptedFileType = {
                                    "image/apng",
                                    "image/bmp",
                                    "image/gif",
                                    "image/jpeg",
                                    "image/pjpeg",
                                    "image/png",
                                    "image/svg+xml",
                                    "image/tiff",
                                    "image/webp",
                                    "image/x-icon"
                                };



        public static async Task<byte[]> GetImageAsByteArray(IBrowserFile file)
        {
            var imageFile = new byte[file.Size];

            if (file.Size <= MaxSize)
            {
                await file.OpenReadStream(maxAllowedSize: 1024 * 1024).ReadAsync(imageFile);
            }
            return imageFile;
        }


        public static bool IsValidImageFile(string fileType)
        {
            return AcceptedFileType.Contains(fileType);
        }
    }       
}

