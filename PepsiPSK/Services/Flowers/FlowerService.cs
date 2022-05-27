using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using Pepsi.Data;
using PepsiPSK.Responses.Service;

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

        public async Task<List<GetFlowerDto>> GetFlowers()
        {
            var flowers = await _context.Flowers.Select(flower => _mapper.Map<GetFlowerDto>(flower)).ToListAsync();
            return flowers;
        }

        public async Task<GetFlowerDto?> GetFlowerById(Guid guid)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);

            if (flower == null)
            {
                return null;
            }

            var mappedFlower = _mapper.Map<GetFlowerDto>(flower);
            return mappedFlower;
        }

        public async Task<GetFlowerDto> AddFlower(AddFlowerDto addFlowerDto)
        {
            Flower newFlower = _mapper.Map<Flower>(addFlowerDto);
            await _context.Flowers.AddAsync(newFlower);
            await _context.SaveChangesAsync();
            var addedFlower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == newFlower.Id);
            var mappedFlower = _mapper.Map<GetFlowerDto>(addedFlower);
            return mappedFlower;
        }

        public async Task<ServiceResponse<GetFlowerDto?>> UpdateFlower(Guid guid, UpdateFlowerDto updateFlowerDto)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);
            var serviceResponse = new ServiceResponse<GetFlowerDto?>();

            if (flower == null)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = $"Flower with ID of {guid} was not found!";

                return serviceResponse;
            }

            if (Nullable.Compare(updateFlowerDto.LastModified, flower.LastModified) != 0)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 409;
                serviceResponse.IsOptimisticLocking = true;
                serviceResponse.Message = $"Flower with ID of {guid} has already been updated!";

                return serviceResponse;
            }

            flower.Name = updateFlowerDto.Name;
            flower.Price = updateFlowerDto.Price;
            flower.Description = updateFlowerDto.Description;
            flower.NumberInStock = updateFlowerDto.NumberInStock;
            flower.LastModified = DateTime.UtcNow;
            _context.Flowers.Update(flower);
            await _context.SaveChangesAsync();
            var mappedFlower = _mapper.Map<GetFlowerDto>(flower);

            serviceResponse.Data = mappedFlower;
            serviceResponse.StatusCode = 200;
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = $"Flower with ID of {guid} was successfully updated!";

            return serviceResponse;
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

        public async Task<ServiceResponse<GetFlowerDto?>> UpdateStock(Guid guid, UpdateStockDto updateStockDto)
        {
            var flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == guid);
            var serviceResponse = new ServiceResponse<GetFlowerDto?>();

            if (flower == null)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = $"Flower with ID of {guid} was not found!";

                return serviceResponse;
            }

            if (Nullable.Compare(updateStockDto.LastModified, flower.LastModified) != 0)
            {
                serviceResponse.Data = null;
                serviceResponse.StatusCode = 500;
                serviceResponse.Message = $"Flower with ID of {guid} has already been updated!";

                return serviceResponse;
            }

            flower.NumberInStock += updateStockDto.FlowerAmount;
            flower.LastModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var mappedFlower = _mapper.Map<GetFlowerDto>(flower);

            serviceResponse.Data = mappedFlower;
            serviceResponse.IsSuccessful = true;
            serviceResponse.StatusCode = 200;
            serviceResponse.Message = $"Stock of flower with ID of {guid} was successfully updated!";

            return serviceResponse;
        }
    }
}