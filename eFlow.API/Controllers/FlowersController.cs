using eFlow.Application.DTOs;
using eFlow.API.Contracts.Flowers;
using eFlow.Application.Services;
using eFlow.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("flowers")]
    public class FlowersController : ControllerBase
    {
        private readonly FlowerService _flowerService;
        public FlowersController(FlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [HttpGet]
        [Route("flowers")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var data = await _flowerService.GetFlowersAsync();
                var response = data.Select(x => new FlowerDTO
                    (x.Id, x.Name, x.Price, x.InStock, x.Description)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(FlowerRequest flowersRequest)
        {
            try
            {
                var data = await _flowerService
                .CreateFlowerAsync(new Flower
                {
                    Name = flowersRequest.Name,
                    Description = flowersRequest.Description,
                    InStock = flowersRequest.InStock,
                    Price = flowersRequest.Price,
                });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }
        [HttpPut]
        [Route("{id}/change")]
        public async Task<IActionResult> ChangeAsync( Guid id, FlowerRequest flowersRequest)
        {
            try
            {
                var data = await _flowerService
                .UpdateFlowerAsync(new Flower
                {
                    Id = id,
                    Name = flowersRequest.Name,
                    Description = flowersRequest.Description,
                    InStock = flowersRequest.InStock,
                    Price = flowersRequest.Price,
                });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("{id}/delete")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var data = await _flowerService.DeleteAsync(id);
                if (!data)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
