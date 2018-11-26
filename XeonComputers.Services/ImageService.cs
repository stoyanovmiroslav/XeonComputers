using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class ImageService : IImageService
    {
        public async void UploadImage(IFormFile formImage, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formImage.CopyToAsync(stream);
            }
        }
    }
}