using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class ImagesService : IImagesService
    {
        public async void UploadImage(IFormFile formImage, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formImage.CopyToAsync(stream);
            }
        }

        public async Task<IEnumerable<string>> UploadImages(IList<IFormFile> formImages, int existingImages, string template, int id)
        {
            var imageUrls = new List<string>();

            for (int i = 0; i < formImages.Count; i++)
            {
                var urlName = $"Id{id}_{existingImages + i}";

                var imagePath = string.Format(template, urlName);
                

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await formImages[i].CopyToAsync(stream);
                }

                var imageRoot = imagePath.Replace(GlobalConstants.WWWROOT, "");
                imageUrls.Add(imageRoot);
            }

            return imageUrls;
        }
    }
}