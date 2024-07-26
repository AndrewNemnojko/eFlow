using eFlow.API.Contracts.Bouquets;
using eFlow.Application.DTOs.Bouquets;
using eFlow.Application.Services;
using eFlow.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace eFlow.API.Controllers
{
    [ApiController]
    [Route("api/bouquets")]
    public class BouquetsController : Controller
    {
        private readonly BouquetService _bouquetService;
        public BouquetsController(BouquetService bouquetService)
        {
            _bouquetService = bouquetService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var data = await _bouquetService.GetBouquetsAsync();
                return Ok(data.Select(x => new BouquetSummaryDTO (x.Id, x.Name, x.BasePrice, x.Available)).ToList());
            }
            catch(Exception ex)
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
                var data = await _bouquetService.GetBouquetById(id);
                if (data == null)
                    return NotFound();
                return Ok(data);
            }
            catch (Exception ex)
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
                var data = await _bouquetService.GetBouquetByName(name);
                if (data == null)
                    return NotFound();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(BouquetRequest bouquetRequest)
        {
            try
            {
                var data = await _bouquetService
                .CreateBouquetAsync(new Bouquet
                {
                    Name = bouquetRequest.Name,
                    Sizes = bouquetRequest.Sizes.Select(x => new BouquetSize
                    {
                        SubName = x.SubName,
                        Flowers = x.Flowers.Select(f => new FlowerQuantity
                        {
                            Flower = new Flower { Id = f.IdFlower },
                            Quantity = f.Quantity,
                        }).ToList()
                    }).ToList()
                });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> ChangeAsync(BouquetRequest bouquetRequest)
        {
            try
            {
                var result = await _bouquetService.ChangeBouquetAsync(new Bouquet
                {
                    Id = bouquetRequest.Id,
                    Name = bouquetRequest.Name,
                    Sizes = bouquetRequest.Sizes.Select(x => new BouquetSize
                    {
                        Id = x.id,
                        SubName = x.SubName,
                        Flowers = x.Flowers.Select(f => new FlowerQuantity
                        {                  
                            Id = f.ID,
                            Flower = new Flower { Id = f.IdFlower },
                            Quantity = f.Quantity,
                        }).ToList()
                    }).ToList()
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAsync(Guid[] Ids)
        {
            try
            {
                var result = await _bouquetService.UpdateBouquetAsync(Ids);
                
                return Ok(result);
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
                var result = await _bouquetService.DeleteAsync(id);                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
