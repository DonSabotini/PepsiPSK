using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Services.Flowers;

namespace PepsiPSK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlowersController : ControllerBase
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlowers()
        {
            var flowers = await _flowerService.GetFlowers();
            return Ok(flowers);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetFlowerById(Guid guid)
        {
            var flower = await _flowerService.GetFlowerById(guid);
            return flower == null ? NotFound() : Ok(flower);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlower(Flower flower)
        {
            var addedFlower = await _flowerService.AddFlower(flower);
            return Ok(addedFlower);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFlower(FlowerDto flowerDto)
        {
            var flower = await _flowerService.UpdateFlower(flowerDto);
            return flower == null ? NotFound() : Ok(flower);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteFlower(Guid guid)
        {
            var successMessage = await _flowerService.DeleteFlower(guid);
            return successMessage == null ? NotFound() : Ok(successMessage);
        }
    }
}
