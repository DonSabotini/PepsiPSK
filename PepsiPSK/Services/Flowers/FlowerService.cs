using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Services.Users;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Flowers
{
    public class FlowerService : IFlowerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public FlowerService(DataContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<Flower>> GetFlowers()
        {
            return await _context.Flowers.Select(flower => flower).ToListAsync();
        }

        public async Task<Flower?> GetFlowerById(Guid guid)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);
            return flower ?? null;
        }

        public async Task<Flower> AddFlower(AddFlowerDto addFlowerDto)
        {
            Flower newFlower = _mapper.Map<Flower>(addFlowerDto);
            newFlower.UserId = _userService.RetrieveCurrentUserId();
            await _context.Flowers.AddAsync(newFlower);
            await _context.SaveChangesAsync();
            var addedFlower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == newFlower.Id);
            return addedFlower;
        }

        public async Task<Flower?> UpdateFlower(UpdateFlowerDto updateFlowerDto)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == updateFlowerDto.Id);

            if (flower == null)
            {
                return null;
            }
          
            flower.Name = updateFlowerDto.Name;
            flower.Price = updateFlowerDto.Price;
            flower.Description = updateFlowerDto.Description;
            _context.Flowers.Update(flower);
            await _context.SaveChangesAsync();

            return flower;
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