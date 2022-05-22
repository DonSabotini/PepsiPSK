using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pepsi.Data;
using PepsiPSK.Entities;
using PepsiPSK.Models.Photos;

namespace PepsiPSK.Services.Photos
{
    public class PhotoService : IPhotoService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PhotoService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Photo?> Get(Guid id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PhotoListDto>> GetAll()
        {
            return await _context.Photos.Select(x => _mapper.Map<PhotoListDto>(x)).ToListAsync();
        }

        public async Task<Photo?> Add(IFormFile image)
        {
            if(image.Length > 0)
            {
                var photo = new Photo();
                photo.Id = Guid.NewGuid();
                photo.ContentType = image.ContentType;
                photo.FileName = image.FileName;

                using (MemoryStream ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    var fileBuff = ms.ToArray();
                    photo.Content = Convert.ToBase64String(fileBuff);
                }

                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                return photo;
            }

            return null;
        }

        public void Delete(Photo photo)
        {
            _context.Photos.Remove(photo);
            _context.SaveChanges();
        }

        public bool Exists(Guid id)
        {
            return _context.Photos.Any(p => p.Id == id);
        }
    }
}
