
using eFlow.Core.Interfaces.Files;
using eFlow.Core.Models;
using eFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace eFlow.Application.Services
{
    public class FlowerService
    {
        private readonly IFlowerRepository _flowerRepository;
        private readonly IMediaFileService _mediaFileService;
        public FlowerService(IFlowerRepository flowerRepository, IMediaFileService mediaFileService)
        {
            _flowerRepository = flowerRepository;
            _mediaFileService = mediaFileService;
        }
        public async Task<Guid> CreateFlowerAsync(Flower flower, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                var mediaFile = await _mediaFileService.UploadAsync(imageFile);
                flower.ImageFile = mediaFile;
            }
            var data = await _flowerRepository.AddAsync(flower);
            return data;
        }
        public async Task<bool> UpdateFlowerAsync(Flower flower, IFormFile? imageFile)
        {
            var existFlower = await _flowerRepository.GetByIdAsync(flower.Id);
            if(existFlower == null) return false;
            
            if (imageFile != null)
            {
                if (existFlower.ImageFile != null)
                {
                    await _mediaFileService.DeleteAsync(existFlower.ImageFile.FileName);
                }
                var mediaFile = await _mediaFileService.UploadAsync(imageFile);
                flower.ImageFile = mediaFile;
            }

            var data = await _flowerRepository.UpdateAsync(flower);
            return data;
        }
        public async Task<IList<Flower>> GetFlowersAsync()
        {
            var data = await _flowerRepository.GetAllAsync();
            return data;
        }
        public async Task<Flower> GetFlowerByName(string name)
        {
            var data = await _flowerRepository.GetByNameAsync(name);
            return data;
        }
        public async Task<Flower> GetFlowerById(Guid id)
        {
            var data = await _flowerRepository.GetByIdAsync(id);
            return data;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existFlower = await _flowerRepository.GetByIdAsync(id);
            if (existFlower != null)
            {
                if (existFlower.ImageFile != null)
                {
                    await _mediaFileService.DeleteAsync(existFlower.ImageFile.FileName);
                }
                var data = await _flowerRepository.RemoveAsync(id);
                return data;
            }
            return false;           
        }
    }
}
