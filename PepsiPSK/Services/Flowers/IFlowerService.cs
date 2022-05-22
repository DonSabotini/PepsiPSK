using PepsiPSK.Models.Flower;

namespace PepsiPSK.Services.Flowers
{
    public interface IFlowerService
    {
        Task<List<GetFlowerDto>> GetFlowers();
        Task<GetFlowerDto?> GetFlowerById(Guid guid);
        Task<GetFlowerDto> AddFlower(AddFlowerDto addFlowerDto);
        Task<GetFlowerDto?> UpdateFlower(Guid guid, UpdateFlowerDto updateFlowerDto);
        Task<string?> DeleteFlower(Guid guid);
        Task<GetFlowerDto?> UpdateStock(Guid guid, UpdateStockDto updateStockDto);
    }
}