using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Services.Contracts
{
    public interface IImageService
    {
        void UploadImage(IFormFile formImage, string path);
    }
}