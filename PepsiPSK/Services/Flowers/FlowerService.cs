using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Flowers
{
    public class FlowerService : IFlowerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FlowerService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FlowerDto>> GetFlowers()
        {
            return await _context.Flowers.Select(flower => _mapper.Map<FlowerDto>(flower)).ToListAsync();
        }

        public async Task<FlowerDto?> GetFlowerById(Guid guid)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);

            if (flower == null)
            {
                return null;
            }

            FlowerDto flowerDto = _mapper.Map<FlowerDto>(flower);
            return flowerDto;
        }

        public async Task<Flower> AddFlower(Flower flower)
        {
            await _context.Flowers.AddAsync(flower);
            await _context.SaveChangesAsync();
            var addedFlower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == flower.Id);
            return addedFlower;
        }

        public async Task<FlowerDto?> UpdateFlower(FlowerDto flowerDto)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == flowerDto.Id);

            if (flower == null)
            {
                return null;
            }
          
            flower.Name = flowerDto.Name;
            flower.Price = flowerDto.Price;
            flower.Description = flowerDto.Description;
            flower.Quantity = flowerDto.Quantity;
            _context.Flowers.Update(flower);
            await _context.SaveChangesAsync();

            FlowerDto updatedFlower = _mapper.Map<FlowerDto>(flower);
            return updatedFlower;
        }

        public async Task<string?> DeleteFlower(Guid guid)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);

            if (flower == null)
            {
                return null;
            }
         
            _context.Flowers.Remove(flower);
            await _context.SaveChangesAsync();
            return "Successfully deleted!";
        }
    }
}