using PepsiPSK.Models.Flower;
using PepsiPSK.Responses.Service;

namespace PepsiPSK.Services.Flowers
{
    public interface IFlowerService
    {
        Task<List<GetFlowerDto>> GetFlowers();
        Task<GetFlowerDto?> GetFlowerById(Guid guid);
        Task<GetFlowerDto> AddFlower(AddFlowerDto addFlowerDto);
        Task<ServiceResponse<GetFlowerDto?>> UpdateFlower(Guid guid, UpdateFlowerDto updateFlowerDto);
        Task<string?> DeleteFlower(Guid guid);
        Task<ServiceResponse<GetFlowerDto?>> UpdateStock(Guid guid, UpdateStockDto updateStockDto);
    }
}