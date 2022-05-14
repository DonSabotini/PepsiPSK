using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Utils.Authentication;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Services.Flowers
{
    public class FlowerService : IFlowerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfoRetriever _currentUserInfoRetriever;

        public FlowerService(DataContext context, IMapper mapper, ICurrentUserInfoRetriever currentUserInfoRetriever)
        {
            _context = context;
            _mapper = mapper;
            _currentUserInfoRetriever = currentUserInfoRetriever;
        }

        private string GetCurrentUserId()
        {
            return _currentUserInfoRetriever.RetrieveCurrentUserId();
        }

        private bool AdminCheck()
        {
            return _currentUserInfoRetriever.CheckIfCurrentUserIsAdmin();
        }

        public async Task<List<Flower>> GetFlowers()
        {
            return AdminCheck() ? await _context.Flowers.Select(flower => flower).ToListAsync() : await _context.Flowers.Where(f => f.UserId == GetCurrentUserId()).ToListAsync();
        }

        public async Task<Flower?> GetFlowerById(Guid guid)
        {
            var flower = AdminCheck() ? await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid) : await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid && f.UserId == GetCurrentUserId());
            return flower ?? null;
        }

        public async Task<Flower> AddFlower(AddFlowerDto addFlowerDto)
        {
            Flower newFlower = _mapper.Map<Flower>(addFlowerDto);
            newFlower.UserId = GetCurrentUserId();
            await _context.Flowers.AddAsync(newFlower);
            await _context.SaveChangesAsync();
            var addedFlower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == newFlower.Id);
            return addedFlower;
        }

        public async Task<Flower?> UpdateFlower(UpdateFlowerDto updateFlowerDto)
        {
            var flower = AdminCheck() ? await _context.Flowers.FirstOrDefaultAsync(f => f.Id == updateFlowerDto.Id) : await _context.Flowers.FirstOrDefaultAsync(f => f.Id == updateFlowerDto.Id && f.UserId == GetCurrentUserId());

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
            var flower = AdminCheck() ? await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid) : await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid && f.UserId == GetCurrentUserId());

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