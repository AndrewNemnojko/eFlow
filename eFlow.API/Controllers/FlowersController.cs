using eFlow.Application.DTOs;
using eFlow.API.Contracts.Flowers;
using eFlow.Application.Services;
using eFlow.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("api/flowers")]
    public class FlowersController : ControllerBase
    {
        private readonly FlowerService _flowerService;
        public FlowersController(FlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [HttpGet]
        [Route("")]
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

        [HttpGet]
        [Route("id={id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _flowerService.GetFlowerById(id);
                if(data == null) NotFound();         
                return Ok(data);    
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("name={name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            try
            {
                var data = await _flowerService.GetFlowerByName(name);
                if (data == null) NotFound();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromForm]FlowerRequest flowersRequest)
        {
            try
            {
                var data = await _flowerService
                .CreateFlowerAsync(new Flower
                {
                    Name = flowersRequest.Name,
                    Description = flowersRequest.Description != null ? flowersRequest.Description : "",
                    InStock = flowersRequest.InStock,
                    Price = flowersRequest.Price,                   
                }, flowersRequest.ImageFile);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> ChangeAsync( Guid id, [FromForm] FlowerRequest flowersRequest)
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
                }, flowersRequest.ImageFile);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
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
