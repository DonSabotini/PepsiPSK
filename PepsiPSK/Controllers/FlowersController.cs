using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PepsiPSK.Models.Flower;
using PepsiPSK.Services.Flowers;

namespace PepsiPSK.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class FlowersController : ControllerBase
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFlowers()
        {
            var flowers = await _flowerService.GetFlowers();
            return Ok(flowers);
        }

        [AllowAnonymous]
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetFlowerById(Guid guid)
        {
            var flower = await _flowerService.GetFlowerById(guid);
            return flower == null ? NotFound() : Ok(flower);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlower(AddFlowerDto addFlowerDto)
        {
            var addedFlower = await _flowerService.AddFlower(addFlowerDto);
            return Ok(addedFlower);
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateFlower(Guid guid, UpdateFlowerDto updateFlowerDto)
        {
            var flower = await _flowerService.UpdateFlower(guid, updateFlowerDto);
            return flower == null ? NotFound() : Ok(flower);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteFlower(Guid guid)
        {
            var successMessage = await _flowerService.DeleteFlower(guid);
            return successMessage == null ? NotFound() : Ok(successMessage);
        }


        [HttpPut("{guid}/update-stock")]
        public async Task<IActionResult> UpdateStock(Guid guid, UpdateStockDto updateStockDto)
        {
            var flower = await _flowerService.UpdateStock(guid, updateStockDto);
            return flower == null ? NotFound() : Ok(flower);
        }
    }
}
