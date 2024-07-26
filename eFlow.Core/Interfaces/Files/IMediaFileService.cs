
using eFlow.Core.Models;
using Microsoft.AspNetCore.Http;

namespace eFlow.Core.Interfaces.Files
{
    public interface IMediaFileService
    {
        Task<MediaFile> UploadAsync(IFormFile file);      
        Task DeleteAsync(string key);
    }
}
