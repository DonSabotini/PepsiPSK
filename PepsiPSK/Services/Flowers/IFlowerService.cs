using PepsiPSK.Models.Flower;

namespace PepsiPSK.Services.Flowers
{
    public interface IFlowerService
    {
        Task<List<GetFlowerDto>> GetFlowers();
        Task<GetFlowerDto?> GetFlowerById(Guid guid);
        Task<GetFlowerDto> AddFlower(AddFlowerDto addFlowerDto);
        Task<GetFlowerDto?> UpdateFlower(UpdateFlowerDto updateFlowerDto);
        Task<string?> DeleteFlower(Guid guid);
    }
}