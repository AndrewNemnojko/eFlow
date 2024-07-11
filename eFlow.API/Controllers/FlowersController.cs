using eFlow.API.Contracts.Flowers;
using eFlow.API.DTOs.Flowers;
using eFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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
                var response = new FlowerResponse<List<FlowerDTO>>
                (
                    true,
                     data.Select(x => new FlowerDTO
                    (x.Name, x.Price, x.InStock, x.Description)).ToList(),
                    "Flowers successful loaded"
                );
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(FlowersRequest<FlowerDTO> flowersRequest)
        {
            try
            {
                var data = await _flowerService
                .CreateFlowerAsync(new Core.Models.Flower
                {
                    Name = flowersRequest.Value.Name,
                    Description = flowersRequest.Value.Description,
                    InStock = flowersRequest.Value.InStock,
                    Price = flowersRequest.Value.Price,
                });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }          
        }
    }
}
