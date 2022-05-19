using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Models.User;
using PepsiPSK.Utils.Authentication;
using Pepsi.Data;

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

        public async Task<List<GetFlowerDto>> GetFlowers()
        {
            var flowers = await _context.Flowers.Select(flower => _mapper.Map<GetFlowerDto>(flower)).ToListAsync();

            foreach(var flower in flowers)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == flower.UserId);
                flower.UserInfo = _mapper.Map<UserInfo>(user);
            }

            return flowers;
        }

        public async Task<GetFlowerDto?> GetFlowerById(Guid guid)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);

            if( flower == null)
            {
                return null;
            }

            var mappedFlower = _mapper.Map<GetFlowerDto>(flower);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedFlower.UserId);
            mappedFlower.UserInfo = _mapper.Map<UserInfo>(user);

            return mappedFlower;
        }

        public async Task<GetFlowerDto> AddFlower(AddFlowerDto addFlowerDto)
        {
            Flower newFlower = _mapper.Map<Flower>(addFlowerDto);
            newFlower.UserId = GetCurrentUserId();
            await _context.Flowers.AddAsync(newFlower);
            await _context.SaveChangesAsync();
            var addedFlower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == newFlower.Id);
            var mappedFlower = _mapper.Map<GetFlowerDto>(addedFlower);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedFlower.UserId);
            mappedFlower.UserInfo = _mapper.Map<UserInfo>(user);

            return mappedFlower;
        }

        public async Task<GetFlowerDto?> UpdateFlower(UpdateFlowerDto updateFlowerDto)
        {
            var flower = AdminCheck() ? await _context.Flowers.FirstOrDefaultAsync(f => f.Id == updateFlowerDto.Id) : await _context.Flowers.FirstOrDefaultAsync(f => f.Id == updateFlowerDto.Id && f.UserId == GetCurrentUserId());

            if (flower == null)
            {
                return null;
            }

            flower.Name = updateFlowerDto.Name;
            flower.Price = updateFlowerDto.Price;
            flower.NumberInStock = updateFlowerDto.NumberInStock;
            flower.Description = updateFlowerDto.Description;
            _context.Flowers.Update(flower);
            await _context.SaveChangesAsync();
            var mappedFlower = _mapper.Map<GetFlowerDto>(flower);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == mappedFlower.UserId);
            mappedFlower.UserInfo = _mapper.Map<UserInfo>(user);

            return mappedFlower;
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