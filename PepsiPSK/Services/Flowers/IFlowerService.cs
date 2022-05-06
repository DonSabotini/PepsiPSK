using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;

namespace PepsiPSK.Services.Flowers
{
    public interface IFlowerService
    {
        Task<List<FlowerDto>> GetFlowers();
        Task<FlowerDto?> GetFlowerById(Guid guid);
        Task<Flower> AddFlower(Flower flower);
        Task<FlowerDto?> UpdateFlower(FlowerDto flowerDto);
        Task<string?> DeleteFlower(Guid guid);
    }
}