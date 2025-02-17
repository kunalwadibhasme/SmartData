using E_CommerceBackend.Interfaces;
using E_CommerceBackend.Services;


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_CommerceBackend.Services
{
    public class ImageFileService : IImageFileService
    {
        private readonly string _webRootPath;

        public ImageFileService(IConfiguration configuration)
        {
            _webRootPath = configuration["WebRootPath"] ?? throw new ArgumentNullException("WebRootPath");
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            var path = Path.Combine(_webRootPath, "profileimages");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            return Path.Combine("Upload", fileName); // Return relative path to access file via URL
        }
    }
}

