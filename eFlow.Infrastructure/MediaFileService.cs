
using eFlow.Core.Interfaces.Files;
using eFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace eFlow.Infrastructure
{
    public class MediaFileService : IMediaFileService
    {
        private readonly string _mediaRootPath;       

        public MediaFileService(IConfiguration configuration, IHostEnvironment env)
        {
            _mediaRootPath = configuration["MediaRootPath"] ?? 
                Path.Combine(env.ContentRootPath, "Media");     
        }

        public async Task<MediaFile> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var directoryPath = Path.Combine(_mediaRootPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new MediaFile
            {
                FileName = fileName,
                FilePath = filePath,
                ContentType = file.ContentType,
                Size = file.Length
            };
        }      

        public Task DeleteAsync(string key)
        {
            var filePath = Path.Combine(_mediaRootPath, key);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }      
    }
}
