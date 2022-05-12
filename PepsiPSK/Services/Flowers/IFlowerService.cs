using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;

namespace PepsiPSK.Services.Flowers
{
    public interface IFlowerService
    {
        Task<List<Flower>> GetFlowers();
        Task<Flower?> GetFlowerById(Guid guid);
        Task<Flower> AddFlower(AddFlowerDto addFlowerDto);
        Task<Flower?> UpdateFlower(UpdateFlowerDto updateFlowerDto);
        Task<string?> DeleteFlower(Guid guid);
    }
}